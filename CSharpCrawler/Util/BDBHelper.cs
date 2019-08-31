using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BerkeleyDB;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace CSharpCrawler.Util
{
    public class BDBHelper
    {
        private BTreeDatabase db;

        public BDBHelper(string dbPath)
        {
            BTreeDatabaseConfig bTreeDatabaseConfig = new BTreeDatabaseConfig();
            //文件不存在则创建
            bTreeDatabaseConfig.Creation = CreatePolicy.IF_NEEDED;
            //页大小
            bTreeDatabaseConfig.PageSize = 512;
            //缓存大小
            bTreeDatabaseConfig.CacheSize = new CacheInfo(0, 64 * 1024, 1);

            db = BTreeDatabase.Open(dbPath, bTreeDatabaseConfig);
        }

        ~BDBHelper()
        {
            if (db != null)
                db.Close();
        }

        public void Put(DatabaseEntry key,DatabaseEntry value)
        {
            db.Put(key, value);
        }

        public void Put(DatabaseEntry key,byte[] data)
        {
            db.Put(key, new DatabaseEntry(data));
        }

        public void Put(DatabaseEntry key,string file)
        {
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    byte[] data = br.ReadBytes((int)fs.Length);                    
                    Put(key, data);
                }
            }
        }

        public KeyValuePair<DatabaseEntry,DatabaseEntry> Get(DatabaseEntry key)
        {
            return db.Get(key);
        }

        public T Get<T>(DatabaseEntry key)
        {
            T t = default(T);
            BinaryFormatter formatter = new BinaryFormatter();        
            MemoryStream memStream;           
            var pair = db.Get(key);
            memStream = new MemoryStream(pair.Value.Data.Length);
            memStream.Write(pair.Value.Data, 0, pair.Value.Data.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            t = (T)formatter.Deserialize(memStream);
            memStream.Close();
            return t;
        }

        public Dictionary<DatabaseEntry,DatabaseEntry> FetchAll()
        {
            BTreeCursor cursor = db.Cursor();
            Dictionary<DatabaseEntry, DatabaseEntry> dic = new Dictionary<DatabaseEntry, DatabaseEntry>();
            
            while(cursor.MoveNext())
            {
                dic.Add(cursor.Current.Key, cursor.Current.Value);
            }

            cursor.Close();
            return dic;
        }

        public void Clear()
        {
            BTreeCursor cursor = db.Cursor();           

            while (cursor.MoveNext())
            {
                db.Delete(cursor.Current.Key);
            }
            cursor.Close();
        }
    }
}
