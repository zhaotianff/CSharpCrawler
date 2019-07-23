--SQLite Version 3
--Using SQLite Release 3.28.0 On 2019-04-16/System.Data.SQLite 1.0.111
--https://www.sqlite.org/changes.html


/*
Navicat SQLite Data Transfer

Source Server         : dianping
Source Server Version : 30706
Source Host           : :0

Target Server Type    : SQLite
Target Server Version : 30706
File Encoding         : 65001

Date: 2019-07-23 09:36:28
*/

PRAGMA foreign_keys = OFF;

-- ----------------------------
-- Table structure for "main"."Province"
-- ----------------------------
DROP TABLE "main"."Province";
CREATE TABLE "Province" (
"ID"  INTEGER NOT NULL,
"Name"  TEXT NOT NULL,
PRIMARY KEY ("ID")
);

-- ----------------------------
-- Records of Province
-- ----------------------------
