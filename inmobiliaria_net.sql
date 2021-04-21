-- phpMyAdmin SQL Dump
-- version 5.0.4
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 21-04-2021 a las 23:51:29
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

DELIMITER $$
--
-- Funciones
--
CREATE DEFINER=`root`@`localhost` FUNCTION `Maxi` (`ultimo` INT) RETURNS INT(11) BEGIN
    DECLARE ultimopago int default 1;
    
    SELECT IFNULL(max(num_pago +1), 1)
    INTO ultimopago
    FROM pagos 
    WHERE ContratoId = ultimo;
    
    RETURN ultimopago;
	
END$$

DELIMITER ;

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
  `id_inquilino` int(10) NOT NULL,
  `estado` int(5) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`id`, `fe_ini`, `fe_fin`, `monto`, `id_inmueble`, `id_inquilino`, `estado`) VALUES
(1, '2021-04-15', '2022-10-16', 960000, 1, 3, 1),
(2, '2021-04-01', '2022-12-31', 50000, 3, 8, 1),
(3, '2021-04-21', '2021-10-21', 50000, 4, 8, 1),
(4, '2021-12-17', '2022-12-31', 205000, 2, 7, 1),
(5, '2021-10-15', '2021-11-15', 44050, 9, 8, 1),
(6, '2021-01-01', '2021-07-01', 34050, 7, 7, 1),
(9, '2021-04-20', '2021-09-30', 506667, 6, 10, 0),
(10, '2021-04-03', '2022-12-31', 200000, 11, 9, 1),
(11, '2021-04-23', '2021-11-30', 34050, 8, 3, 1),
(12, '2021-01-01', '2021-12-31', 8000, 5, 9, 1);

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
  `estado` int(5) NOT NULL DEFAULT 1,
  `foto` varchar(250) COLLATE utf8_spanish_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`id_Inmu`, `direccion_in`, `uso`, `tipo`, `ambientes`, `precio`, `id_propietario`, `estado`, `foto`) VALUES
(1, 'El cipres 100', 'temporal', 'deposito', 2, 8000, 4, 1, '/img/fotos1/frente.jpg'),
(2, 'Av Mercau 250', 'residencia', 'departamento', 4, 30000, 8, 0, '/img/fotos2/frente.jpg'),
(3, 'Los Incas 3444', 'Cabaña', 'Hogar', 5, 12500, 4, 1, '/img/fotos3/frente.jpg'),
(4, 'Jkoslay - Ruta 20', 'Deportivo', 'Canchas', 6, 17000, 6, 1, '/img/fotos4/frente.jpg'),
(5, 'Barranca Colorada', 'Cabaña', 'Hogar', 5, 25000, 11, 1, '/img/fotos5/frente.jpg'),
(6, 'Cerro Aspero', 'Deportivo', 'Club', 10, 300000, 9, 1, '/img/fotos6/frente.jpg'),
(7, 'Chumamaya', 'Familiar', 'Chalet', 7, 50000, 10, 1, '\\img/fotos10\\Inmueble_10.jpg'),
(8, 'Chumamaya lote5', 'Cabaña', 'Hogar', 5, 34000, 4, 1, '/img/fotos4/Inmueble_4.jpg'),
(10, 'Piedra Blanca Este', 'Social', 'Club', 8, 100000, 12, 1, '/img/fotos12\\Inmueble_19_04_2021.jpg'),
(11, 'Ptro. Becerra 210', 'Familiar', 'Dpto', 4, 46000, 12, 1, '/img/fotos12\\Inmueble_19_04_202107_44_48.jpg');

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
(3, 22457884, 'Marcial Jasinto', 'marcial@mail.com', 'Nueva Galia sur', 482027, 'Kimberly', 'Juan Zalsedo', 7849837, 23344657),
(5, 123456, 'Mary Campos', 'mary@mail.com', 'Rios de los Sauces - Cba', 9735723, 'Jkoslay Inc.', 'Ana Roganovich', 44545453, 54532234),
(7, 1928374, 'Agostina Argento', 'argento@mail.com', 'Albardon - SJ', 345632, 'Bodegas INCA', 'Clarisa Camargo', 9999292, 3453748),
(8, 34783759, 'Alex Baldez', 'alex@gmail.com', '500 Sur', 38756838, 'Autonomo', 'Leo Baldez', 56456456, 456646456),
(9, 8658782, 'Luisa Devia', 'devia@mail.com', 'Mundial 78', 47565, 'Artesanias LU', 'Lucas Arrieta', 67858, 247843),
(10, 3476845, 'Paolo DoSanto', 'paolo@mail.com', 'Brazil', 836823, 'Playa', 'Andreina Leiva', 7687387, 7474738),
(11, 646799, 'Jose Arnaldo', 'arnaldo@mail.com', 'Los quebrachos 32', 6473895, 'Antie', 'Lucas Soliz', 77583893, 46384739),
(12, 658748, 'Frida Duarte', 'duarte@mail.com', 'B170 M51 C47', 6387384, 'UNSL', 'Goyo Duarte', 7384938, 463848);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pagos`
--

CREATE TABLE `pagos` (
  `id` int(10) NOT NULL,
  `num_pago` int(50) NOT NULL,
  `fecha` date NOT NULL,
  `importe` decimal(15,0) NOT NULL,
  `ContratoId` int(10) NOT NULL,
  `estado` int(5) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `pagos`
--

INSERT INTO `pagos` (`id`, `num_pago`, `fecha`, `importe`, `ContratoId`, `estado`) VALUES
(1, 1, '2021-04-13', '44050', 5, 0),
(2, 2, '2021-04-16', '67000', 2, 1),
(3, 1, '2021-04-14', '10050', 10, 1),
(4, 1, '2021-04-24', '99944', 6, 1),
(5, 2, '2021-04-17', '34522', 10, 1),
(6, 1, '2021-04-18', '8000', 12, 1),
(7, 1, '2021-04-19', '12000', 11, 1),
(8, 2, '2021-05-18', '8000', 12, 1),
(10, 2, '2021-04-21', '34050', 6, 1),
(11, 1, '2021-04-23', '205000', 4, 1),
(12, 1, '2021-04-30', '50000', 3, 1);

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
(8, 'leonel baldez', 80956675, '500 Sur m171 c1', 9999999),
(9, 'fulano mengano', 26736788, 'juana koslay', 4764487),
(10, 'Molina Campos', 789, 'La Pampa', 35189345),
(11, 'Teodoro Albornoz', 97364654, 'Pampa de Achala', 2489467),
(12, 'Marina Bustos', 675839, 'Av del Sol 101', 573839),
(13, 'Silvina Navarro', 9374658, 'Los teros s/n', 434393);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `id` int(10) NOT NULL,
  `Nombre` varchar(50) COLLATE utf8_spanish_ci NOT NULL,
  `Apellido` varchar(50) COLLATE utf8_spanish_ci NOT NULL,
  `Avatar` varchar(50) COLLATE utf8_spanish_ci NOT NULL DEFAULT '/img/default.jpg',
  `Mail` varchar(50) COLLATE utf8_spanish_ci NOT NULL,
  `Clave` varchar(100) COLLATE utf8_spanish_ci NOT NULL,
  `pregunta` varchar(150) COLLATE utf8_spanish_ci NOT NULL,
  `Rol` int(10) NOT NULL,
  `estado` int(5) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`id`, `Nombre`, `Apellido`, `Avatar`, `Mail`, `Clave`, `pregunta`, `Rol`, `estado`) VALUES
(2, 'Admin', 'Admin', '/img/avatars/admin.jpg', 'admin@mail.com', 'Od5l8kFX8vwvOF5BPAZjFy6qbs/5m9WgQzPkKL1TqA8=', 'admin', 1, 1),
(8, 'Yanina', 'Sarmiento', '/img/avatars/avatar_yani@mail.com.jpg', 'yani@mail.com', 'O/GNVCJ4WQOmY9isoyQQcW0RuNMdL2oZjHij8JUvyh4=', 'yani', 2, 1),
(9, 'Lorenzo', 'Rosales', '/img/default.jpg', 'rosales@mail.com', 'xgoKeIJD+VeM6HIFoQArXwHk/grZSiA1bY5N79w2RPQ=', 'rosales', 2, 0),
(10, 'Dante', 'Gaudes', '/img/avatars/avatar_gaude@mail.com.jpg', 'gaude@mail.com', '1YTD46gt2Z3a7nHRAD1xOuveWcFwr6S74ase1jkD1Fw=', 'gaude', 2, 1),
(11, 'Ramoncito', 'Alba', '/img/avatars\\avatar_alba@mail.com.jpg', 'alba@mail.com', 'SM7sXEHBQIopzhSXLc7G8ZcIbwqY/ueyo5rS8+R6MCM=', 'alba', 2, 1),
(12, 'Mara', 'Cuello', '/img/avatars\\avatar_cuello@mail.com.jpg', 'cuello@mail.com', 'SM7sXEHBQIopzhSXLc7G8ZcIbwqY/ueyo5rS8+R6MCM=', 'cuello', 2, 1),
(13, 'Walter', 'Oviedo', '/img/avatars\\avatar_aviedo@mail.com.jpg', 'aviedo@mail.com', 'SM7sXEHBQIopzhSXLc7G8ZcIbwqY/ueyo5rS8+R6MCM=', 'aviedo', 2, 1),
(14, 'Roxi', 'Almeida', '/img/avatars\\avatar_almeida@mail.com.jpg', 'almeida@mail.com', 'SM7sXEHBQIopzhSXLc7G8ZcIbwqY/ueyo5rS8+R6MCM=', 'almeida', 1, 1);

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
-- Indices de la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contrato`
--
ALTER TABLE `contrato`
  MODIFY `id` int(5) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `id_Inmu` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `id` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT de la tabla `pagos`
--
ALTER TABLE `pagos`
  MODIFY `id` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `id` int(5) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `id` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
