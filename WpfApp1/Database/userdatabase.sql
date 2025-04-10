-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2025. Már 09. 13:43
-- Kiszolgáló verziója: 10.4.32-MariaDB
-- PHP verzió: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `userdatabase`
--
CREATE DATABASE IF NOT EXISTS `userdatabase` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `userdatabase`;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `accesslevels`
--

DROP TABLE IF EXISTS `accesslevels`;
CREATE TABLE `accesslevels` (
  `AccessID` int(11) NOT NULL,
  `AccessLVL` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `accesslevels`
--

INSERT INTO `accesslevels` (`AccessID`, `AccessLVL`) VALUES
(0, 'User'),
(1, 'Admin'),
(2, 'Owner');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `users`
--

DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
  `id` int(11) NOT NULL,
  `username` varchar(50) NOT NULL,
  `email` varchar(100) NOT NULL,
  `password` varchar(255) NOT NULL,
  `AccessID` int(11) DEFAULT 0 CHECK (`AccessID` between 0 and 3),
  `created_at` timestamp NOT NULL DEFAULT current_timestamp()
) ;

--
-- A tábla adatainak kiíratása `users`
--

INSERT INTO `users` (`id`, `username`, `email`, `password`, `AccessID`, `created_at`) VALUES
(1, 'adresz', 'tigerad97@gmail.com', '$2b$12$9lJ7aNZPX8H/dOP2C.20Z.4eeHB2q1vYHgwj9wst4U8cSOp5pc/FG', 2, '2025-02-27 20:03:22'),
(2, 'm.zeteny', 'meszaros.zeteny@gmail.com', '$2b$12$LG656O9OvlGrv5NT1sv1k.N5k9KNED650f97XcPBljMrYaLT8EVY2', 2, '2025-02-27 20:15:47'),
(3, 'sz.arpi', 'szabo.arpad@gmail.com', '$2b$12$tzDf6LPQLRbY.UNkBQqH4.8sBIVa4.1gcfx6hx0JqdWIyhKHEeFja', 2, '2025-02-27 20:16:01');

--
-- Eseményindítók `users`
--
DROP TRIGGER IF EXISTS `before_insert_users`;
DELIMITER $$
CREATE TRIGGER `before_insert_users` BEFORE INSERT ON `users` FOR EACH ROW BEGIN
    SET NEW.username = LOWER(NEW.username);
    SET NEW.email = LOWER(NEW.email);
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `user_details`
--

DROP TABLE IF EXISTS `user_details`;
CREATE TABLE `user_details` (
  `email` varchar(255) NOT NULL,
  `First_Name` varchar(100) NOT NULL,
  `Last_Name` varchar(100) NOT NULL,
  `Phone_Number` varchar(11) NOT NULL,
  `TAJ_Number` varchar(9) NOT NULL,
  `Birth_Date` date DEFAULT NULL,
  `IsBanned` tinyint(1) NOT NULL DEFAULT 0,
  `Ban_Reason` varchar(200) DEFAULT "Nincs tiltva.",
  `Gender` enum('Male','Female') DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `user_details`
--

INSERT INTO `user_details` (`email`, `First_Name`, `Last_Name`, `Phone_Number`, `TAJ_Number`, `Birth_Date`, `IsBanned`, `Ban_Reason`, `Gender`) VALUES
('meszaros.zeteny@gmail.com', 'Zétény', 'Mészáros', '06203124396', '720831921', '2005-02-28', 0, "Nincs tiltva.", 'Male'),
('szabo.arpad@gmail.com', 'Árpád', 'Szabó', '06302786491', '720932912', '2002-02-02', 0, "Nincs tiltva.", 'Male'),
('tigerad97@gmail.com', 'Adrián', 'Tiger', '06709319291', '720952831', '2002-12-10', 0, "Nincs tiltva.", 'Male');

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `accesslevels`
--
ALTER TABLE `accesslevels`
  ADD PRIMARY KEY (`AccessID`);

--
-- A tábla indexei `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `username` (`username`),
  ADD UNIQUE KEY `email` (`email`),
  ADD UNIQUE KEY `username_2` (`username`),
  ADD UNIQUE KEY `email_2` (`email`),
  ADD KEY `AccessID` (`AccessID`);

--
-- A tábla indexei `user_details`
--
ALTER TABLE `user_details`
  ADD PRIMARY KEY (`email`),
  ADD UNIQUE KEY `email` (`email`),
  ADD UNIQUE KEY `unique_email` (`email`),
  ADD UNIQUE KEY `ID_Number` (`TAJ_Number`),
  ADD UNIQUE KEY `Phone_Number` (`Phone_Number`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `users`
--
ALTER TABLE `users`
  ADD CONSTRAINT `users_ibfk_1` FOREIGN KEY (`AccessID`) REFERENCES `accesslevels` (`AccessID`);

--
-- Megkötések a táblához `user_details`
--
ALTER TABLE `user_details`
  ADD CONSTRAINT `user_details_ibfk_1` FOREIGN KEY (`email`) REFERENCES `users` (`email`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
