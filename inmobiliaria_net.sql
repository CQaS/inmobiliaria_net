-- phpMyAdmin SQL Dump
-- version 5.0.4
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 09-04-2021 a las 20:45:30
-- Versión del servidor: 10.4.17-MariaDB
-- Versión de PHP: 8.0.2

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliaria_net`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contrato`
--

CREATE TABLE `contrato` (
  `id` int(5) NOT NULL,
  `fe_ini` date NOT NULL,
  `fe_fin` date NOT NULL,
  `monto` int(11) NOT NULL,
  `id_inmueble` int(10) NOT NULL,
  `id_inquilino` int(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmueble`
--

CREATE TABLE `inmueble` (
  `id_Inmu` int(10) NOT NULL,
  `direccion_in` varchar(100) COLLATE utf8_spanish_ci NOT NULL,
  `uso` varchar(10) COLLATE utf8_spanish_ci NOT NULL,
  `tipo` varchar(15) COLLATE utf8_spanish_ci NOT NULL,
  `ambientes` int(5) NOT NULL,
  `precio` int(10) NOT NULL,
  `id_propietario` int(5) NOT NULL,
  `estado` int(5) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`id_Inmu`, `direccion_in`, `uso`, `tipo`, `ambientes`, `precio`, `id_propietario`, `estado`) VALUES
(1, 'El cipres 100', 'temporal', 'deposito', 2, 8000, 4, 1),
(2, 'Av Mercau 250', 'residencia', 'departamento', 4, 30000, 8, 1),
(3, 'Los Incas 3444', 'Cabaña', 'Hogar', 5, 12500, 4, 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilinos`
--

CREATE TABLE `inquilinos` (
  `id` int(10) NOT NULL,
  `Dni` int(50) NOT NULL,
  `Nombre` varchar(100) COLLATE utf8_spanish_ci NOT NULL,
  `mail` varchar(100) COLLATE utf8_spanish_ci NOT NULL,
  `direccion` varchar(100) COLLATE utf8_spanish_ci NOT NULL,
  `tel_inquilino` int(20) NOT NULL,
  `lugarTrabajo` varchar(100) COLLATE utf8_spanish_ci NOT NULL,
  `nom_garante` varchar(50) COLLATE utf8_spanish_ci NOT NULL,
  `dni_garante` int(15) NOT NULL,
  `tel_garante` int(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `inquilinos`
--

INSERT INTO `inquilinos` (`id`, `Dni`, `Nombre`, `mail`, `direccion`, `tel_inquilino`, `lugarTrabajo`, `nom_garante`, `dni_garante`, `tel_garante`) VALUES
(3, 58457884, 'Marcial', 'marcial@mail.com', 'Nueva Galia', 482027, 'Kimberly', 'Juan Zalsedo', 7849837, 23344657),
(5, 123456, 'Mary Campos', 'mary@mail.com', 'Rios de los Sauces - Cba', 9735723, 'Jkoslay Inc.', 'Ana Roganovich', 44545453, 54532234),
(7, 1928374, 'Agostina Argento', 'argento@mail.com', 'Albardon - SJ', 345632, 'Bodegas INCA', 'Clarisa Camargo', 9999292, 3453748),
(8, 34783759, 'Alex Baldez', 'alex@gmail.com', '500 Sur', 38756838, 'Autonomo', 'Leo Baldez', 56456456, 456646456);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pagos`
--

CREATE TABLE `pagos` (
  `id` int(10) NOT NULL,
  `num_pa` int(50) NOT NULL,
  `fecha` date NOT NULL,
  `importe` int(15) NOT NULL,
  `id_inquilino` int(10) NOT NULL,
  `id_inmueble` int(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietarios`
--

CREATE TABLE `propietarios` (
  `id` int(5) NOT NULL,
  `Nombre` varchar(50) COLLATE utf8_spanish_ci NOT NULL,
  `Dni` int(20) NOT NULL,
  `Direccion` varchar(100) COLLATE utf8_spanish_ci NOT NULL,
  `tel` int(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `propietarios`
--

INSERT INTO `propietarios` (`id`, `Nombre`, `Dni`, `Direccion`, `tel`) VALUES
(3, 'Zabala Nelida', 9968476, 'La Punta', 784392),
(4, 'Romagnolli Antonella', 6644667, 'San Luis Cap', 334567),
(5, 'Faustino Sosa', 65746574, 'Los Cocos', 256348),
(6, 'Gustavo Bogado', 5181575, 'Cortaderas', 837463),
(7, 'Marisa Zapata', 6474655, 'Los estribos 501', 26688999),
(8, 'leonel baldez', 80956675, '500 Sur m171 c1', 9999999),
(9, 'fulano mengano', 26736788, 'juana koslay', 4764487);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD PRIMARY KEY (`id_Inmu`);

--
-- Indices de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contrato`
--
ALTER TABLE `contrato`
  MODIFY `id` int(5) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `id_Inmu` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `id` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `id` int(5) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
