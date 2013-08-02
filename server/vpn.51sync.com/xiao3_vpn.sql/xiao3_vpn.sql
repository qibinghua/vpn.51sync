-- phpMyAdmin SQL Dump
-- version 3.3.10.4
-- http://www.phpmyadmin.net
--
-- 主机: localhost
-- 生成日期: 2012 年 10 月 19 日 14:09
-- 服务器版本: 5.5.27
-- PHP 版本: 5.4.7

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- 数据库: `xiao3_vpn`
--

-- --------------------------------------------------------

--
-- 表的结构 `vpn_account`
--

CREATE TABLE IF NOT EXISTS `vpn_account` (
  `user_id` int(11) NOT NULL AUTO_INCREMENT,
  `user_mail` varchar(128) NOT NULL,
  `user_pass` varchar(40) NOT NULL,
  `user_ip` varchar(15) NOT NULL DEFAULT '0',
  `is_locked` enum('0','1') NOT NULL DEFAULT '0',
  `verify_hash` varchar(32) NOT NULL DEFAULT '0',
  `created` int(11) NOT NULL,
  `updated` int(11) NOT NULL,
  PRIMARY KEY (`user_id`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8 AUTO_INCREMENT=18 ;

-- --------------------------------------------------------

--
-- 表的结构 `vpn_invoice`
--

CREATE TABLE IF NOT EXISTS `vpn_invoice` (
  `order_id` int(11) NOT NULL,
  `order_number` varchar(32) CHARACTER SET utf8 NOT NULL,
  `buyer_email` varchar(128) CHARACTER SET utf8 NOT NULL DEFAULT '0',
  `total_fee` float NOT NULL,
  `trade_time` int(11) NOT NULL,
  `trade_status` varchar(128) CHARACTER SET utf8 NOT NULL DEFAULT '0',
  `trade_no` varchar(64) CHARACTER SET utf8 NOT NULL DEFAULT '0',
  `trade_ip` varchar(15) CHARACTER SET utf8 NOT NULL DEFAULT '0',
  `per_day` int(11) NOT NULL DEFAULT '30',
  `due_time` int(11) NOT NULL DEFAULT '0',
  `is_expired` enum('0','1') CHARACTER SET utf8 NOT NULL DEFAULT '0',
  `created` int(11) NOT NULL,
  `updated` int(11) NOT NULL,
  PRIMARY KEY (`order_number`),
  KEY `order_id` (`order_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- 表的结构 `vpn_order`
--

CREATE TABLE IF NOT EXISTS `vpn_order` (
  `order_id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL,
  `plan_id` int(11) NOT NULL DEFAULT '0',
  `name` varchar(128) CHARACTER SET utf8 NOT NULL,
  `username` varchar(16) CHARACTER SET utf8 NOT NULL,
  `password` varchar(32) CHARACTER SET utf8 NOT NULL,
  `groupname` varchar(16) CHARACTER SET utf8 NOT NULL DEFAULT 'NOR',
  `billingcycle` enum('monthly','quarterly','semi-annually','annually') COLLATE utf8_unicode_ci NOT NULL DEFAULT 'monthly',
  `status` enum('unpaid','pending','approve','canceled','fraud','refund','declined','expired') CHARACTER SET utf8 NOT NULL DEFAULT 'unpaid',
  `cost` float NOT NULL,
  `created` int(11) NOT NULL,
  PRIMARY KEY (`order_id`),
  UNIQUE KEY `username` (`username`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci AUTO_INCREMENT=7 ;

-- --------------------------------------------------------

--
-- 表的结构 `vpn_plans`
--

CREATE TABLE IF NOT EXISTS `vpn_plans` (
  `plan_id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(128) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `desc` text CHARACTER SET utf8 COLLATE utf8_unicode_ci,
  `groupname` varchar(32) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `sku` text CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  PRIMARY KEY (`plan_id`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8 AUTO_INCREMENT=3 ;
