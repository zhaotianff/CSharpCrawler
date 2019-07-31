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

Date: 2019-07-31 10:43:16
*/

PRAGMA foreign_keys = OFF;

-- ----------------------------
-- Table structure for "main"."City"
-- ----------------------------
DROP TABLE "main"."City";
CREATE TABLE "City" (
"ProvinceID"  INTEGER NOT NULL,
"CityID"  INTEGER NOT NULL,
"CityName"  TEXT NOT NULL,
"CityPinYin"  TEXT NOT NULL,
CONSTRAINT "PROVINCE_FOREIGN_KEY" FOREIGN KEY ("ProvinceID") REFERENCES "Province" ("ID")
);

-- ----------------------------
-- Records of City
-- ----------------------------
