/*
--SQLite Version 3
--Using SQLite Release 3.28.0 On 2019-04-16/System.Data.SQLite 1.0.111
--https://www.sqlite.org/changes.html

Navicat SQLite Data Transfer

Source Server         : dianping
Source Server Version : 30706
Source Host           : :0

Target Server Type    : SQLite
Target Server Version : 30706
File Encoding         : 65001

Date: 2019-07-31 10:43:30
*/

PRAGMA foreign_keys = OFF;

-- ----------------------------
-- Table structure for "main"."Result"
-- ----------------------------
DROP TABLE "main"."Result";
CREATE TABLE "Result" (
"CityID"  INTEGER NOT NULL,
"Html"  TEXT NOT NULL,
"RestaurentName"  TEXT NOT NULL,
"AverageSpend"  REAL NOT NULL,
"RestaurentImage"  BLOB,
"Keyword"  TEXT NOT NULL,
CONSTRAINT "CITY_FOREIGN_KEY" FOREIGN KEY ("CityID") REFERENCES "City" ("CityID")
);

-- ----------------------------
-- Records of Result
-- ----------------------------
