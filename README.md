## Consulta 1 
DEVUELVA UN LISTADO CON EL CODIGO DE PEDIDO,CODIGO DE CLIENTE,FECHA DE ESPERA Y DE ENTREGA DE LOS PEDIDOS QUE NO HAN SIDO ENTREGADOS A TIEMPO 

### ENDPOINT = http://localhost:5052/api/Pedido/Consulta1


### CODIGO DE LA CONSULTA =  

public async Task<object> Consulta1()
        {
            var consulta = from p in _context.Pedidos
                           where p.FechaEsperada < p.FechaEntrega
                           select new
                           {
                               CodigoDePedido = p.CodigoPedido,
                               CodigoDeCliente = p.CodigoCliente,
                               FechaEsperada = p.FechaEsperada,
                               FechaEntrega = p.FechaEntrega,
                               Informacion = "No fue entregado a tiempo"
                           };

            var resultado = await consulta.ToListAsync();
            return resultado;
        }

## EXPLICACION 
ENTRAMOS A LA TABLA PEDIDOS DONDE ACCEDEMOS A LOS DOS ATRIBUTOS DE LAS FECHAS , DONDE DAMOS CON LA CONDICION DE QUE LA FECHA ESPERADA TIENE QUE SER MENOR A LA FECHA DE ENTREGA PARA SEÑALAR QUE FUE ENTREGADA A TIEMPO, CREAMOS EL OBJETO DE CONSULTA DONDE SE VAN A IMPRIMIR TODOS LOS ATRIBUTOS QUE QUIEREN QUE SE MUESTREN Y LOS IMPRIMIMOS 


## CONSULTA 2 = DEVUELVE EL NOMBRE DE LOS CLIENTES QUE NO HAN HECHO PAGOS Y EL NOMBRE DE SUS REPRESENTANTES JUNTO CON LA CIUDAD DE LA OFICINA A LA QUE PERTENECE EL REPRESENTANTE 

## ENDPOINT = http://localhost:5052/api/Cliente/Consulta

## CODIGO DE LA CONSULTA == 

public async Task<IEnumerable<object>> Consulta2()
        {
            var mensaje = "Nombre de los clientes que no han hecho pagos y el nombre de sus representantes junto con la ciudad de la oficina".ToUpper();

            var consulta = from cliente in _context.Clientes
                           join representante in _context.Empleados on cliente.CodigoEmpleadoRepVentas equals representante.CodigoEmpleado
                           join oficina in _context.Oficinas on representante.CodigoOficina equals oficina.CodigoOficina
                           where !_context.Pagos.Any(pago => pago.CodigoCliente == cliente.CodigoCliente)
                           select new
                           {
                               cliente.NombreCliente,
                               Representante = new
                               {
                                   representante.Nombre,
                                   representante.Apellido1,
                                   Oficina = new
                                   {
                                       oficina.Ciudad

                                   }
                               }
                           };

            var resultado = new List<object>
    {
        new { Informacion = mensaje, Resultado = await consulta.ToListAsync() }
    };

            return resultado;
        }

## EXPLICACION = 
ACCEDEMOS A LA TABLA DE CLIENTES Y HACEMOS LA RELACION CON LA TABLA DE EMPLEADOS CON EL EQUALS, DESPUES INGRESAMOS A LA TABLA OFICINA QUE HACE LA RELACION CON ESRTA MISMA DONDE EL REPRESENTATE DEBE COINCIDIR CON EL CODIGO DE OFICINA, DESPUES HACEMOS LA CONDICION DONDE LE DECIMOS QUE SI NO HAY ALGUN PAGO QUE ESTE RELACIONADOI CON EL CODIGO CLIENTE , SE CREA EL OBJETO CON LA INFORMACION DE CLIENTE Y REPRESENTANTE Y SE IMPRIME


## Consulta 4 = DEVUELVE UN LISTADO DE LOS 20 PRODUCTOS MAS VENDIDOS Y EL NUMERO TOTAL DE UNIDADES QUE SE HAN VENDIDO DE CADA UNO , EL LISTADO DEBERA ESTAR ORDENADO POR EL NUMERO TOTAL DE UNIDADES VENDIDAD



## ENDPOINT = http://localhost:5052/api/DetallePedido/Consulta4



## CODIGO DE LA CONSULTA 
 public async Task<IEnumerable<object>> Consulta4()
                {
                    var consulta = await (
                        from grupoDetalles in _context.DetallePedidos
                            .GroupBy(detalle => detalle.CodigoProducto)
                            .Select(grupoDetalles => new
                            {
                                CodigoProducto = grupoDetalles.Key,
                                UnidadesVendidas = grupoDetalles.Sum(detalle => detalle.Cantidad)
                            })
                            .OrderByDescending(resultado => resultado.UnidadesVendidas)
                            .Take(20)
                        join producto in _context.Productos
                            on grupoDetalles.CodigoProducto equals producto.CodigoProducto
                        select new
                        {
                            grupoDetalles.CodigoProducto,
                            producto.Nombre,
                            grupoDetalles.UnidadesVendidas
                        })
                        .ToListAsync();

                    return consulta;
                }


## EXPLICACION = ACCEDEMOS A LA TABLA DE DETALLES PEDIDOS, DONDE CON EL GROUPOBY LAS VAMOS A AGRUPAR POR EL CODIGO DEL PRODUCTO CON EL DETALLE, LUEGO SELECCIONAMOS ESA TABLA FICTICIA CREADA EN LA CONSULTA Y LO QUE HACEMOS ES HACER LA RELACION DE CADA UNA DE ESTAS TOMANDO COMO EL ATRIBUTO DE LA TABLA FICTICIADA EL KEY , LUEGO SUMAMOS LAS CANTIDADES PARA DAR CON LAS UNIDADES VENDIDAS , LUEGO CPON EL METODO TAKE TOMAMOS 20 UNIDADES VENDIDAS DE MANERA DECENDENTE  Y CREAMOS EL OBJETO PARA SU IMPRESION 

## CONSULTA 5 = LISTA LAS VENTAS TOTALES DE LOS PRODUCTOS QUE HAYA FACTURADO MAS DE 3000 EUROS, SE MOSTRARA EL NOMBRE UNIDADES VENDIDAS, TOTAL FACTURADO Y TOTAL FACTURADO CON IMPUESTO 

## ENDPOINT = http://localhost:5052/api/DetallePedido/Consulta5


## CODIGO DE LA CONSULTA = 


public async Task<IEnumerable<object>> Consulta5()
            {
                var consulta = await _context.DetallePedidos
                    .GroupBy(detallePedido => detallePedido.CodigoProducto)
                    .ToListAsync();
                
                var _consulta =  consulta
                    .Join(
                        _context.Productos,
                        grupo => grupo.Key,
                        producto => producto.CodigoProducto,
                        (consulta, producto) => new
                        {
                            NombreProducto = producto.Nombre,
                            UnidadesVendidad = consulta.Sum(p => p.Cantidad),
                            TotalFacturado = consulta.Sum(p => p.Cantidad * p.PrecioUnidad),
                            TotalFacturadoIva = consulta.Sum(p => p.Cantidad * p.PrecioUnidad * (decimal)1.21)
                        })
                        .Where(resultado => resultado.TotalFacturadoIva > 3000)
                        .OrderByDescending(resultado => resultado.TotalFacturadoIva)
                        .ToList();

                    return _consulta;
                ;
            }



## EXPLICACION = ACCEDEMOS A LA TABLA DETALLE PEDIDOS DOND EVAMOS A CREAR UNA TABLA FICTICIA HACIENDPO UN JOIN DONDE HACEMOS LA RELACION DE CODIGO PRODUCTO CON EL PRODUCTO Y HA ESTA TABLA LA CREAMOS COMO UN OBJETO PA¿SANTO TODAS LA CONVERSIONES QUE SE NECESITAN COMO EL TOTAL FACTURADO Y EL CON IVA, DESPUES HACEMOS LA CONDICION QUE SOLO DONDE SEA MAYOR A 3000 LOS ORDENE DESENDENTEMENTE EL OBJETO CREADO 


## CONSULTA 6 = DEVULEVE EL NOMBRE DEL PRODUCTO DEL QUE SE HAN VENDIDO MAS UNIDADES

## ENDPOINT = http://localhost:5052/api/Producto/Consulta6



## CODIGO DE LA CONSULTA = 

public async Task<string> Consulta6()
                {
                    var nombreProducto = await _context.Productos
                        .OrderByDescending(p => p.DetallePedidos.Sum(dp => dp.Cantidad))
                        .Select(p => p.Nombre)
                        .FirstOrDefaultAsync();

                    return nombreProducto ?? "No hay productos vendidos";
                }

## EXPLICACION = ENTRAMOS A LA TABLA PRODCUTOS Y LO QUE HACEMOS ES DE MANERA ORDENADA DECENDENTEMENTE CALMULAMOS LA SUMA DE LA TABLA DE DETALLES PEDIDOS LA CANTIDAD DE PRODUCTOS QUE SE HAN VENDIDO, SELECCIONAMOS EL NOMBRE PARA QUE SE IMPRIMA Y LO HACEMOS ASINCRONICAMENTE , AL FINAL PONEMOS UNA CONDICION DE QUE SI AL RETORNAR ES VACIO ESTE OBJETO NOS RETORE QUE NO HAY PRODUCTOS VENDIDOS 










## consulta 10 = DEVUELVE UN LISTADO DE LOS PRODUCTOS QUE NUNCA HAN APARECIDO EN UN PEDIDO 

## ENDPOINT = http://localhost:5052/api/Producto/Consulta10


## CODIGO DE LA CONSULTA = 
        public async Task<IEnumerable<object>> Consulta10()
        {
            var mensaje = "Productos que nunca han aparecido en un pedido".ToUpper();

            var consulta = from producto in _context.Productos
                           join detallePedido in _context.DetallePedidos on producto.CodigoProducto equals detallePedido.CodigoProducto into detallesPedidosJoin
                           from detallePedido in detallesPedidosJoin.DefaultIfEmpty()
                           where detallePedido == null
                           select new
                           {
                               Producto = new
                               {
                                   Nombre = producto.Nombre,
                                   Descripcion = producto.Descripcion,
                                   Imagen = producto.GamaNavigation.Image  // Utilizamos la propiedad Image de GamaProducto
                               }
                           };

            var resultado = new List<object>
    {
        new { Informacion = mensaje, Resultado = await consulta.ToListAsync() }
    };

            return resultado;
        }


## explicacion = ENTRAMOS A LA TABLA PRODUCTOS DONDE CON UN JOIN LO QUE HACEMOS ES QUE LOS DETALLES PEDIDOS EN LA TABLA DE PRODUCTOS COINCIDAN, DONDE LA CONDICION WHERE HACE QUE SI ESTE DETALLE PEDIDO ES NULL SE CREA EL OBJETO PRODUCTO CON LOS ATRIBUTOS QUE LE COLOCAMOS Y QUE SE IMPRIMAN ESTOS # JardineriaFiltroFinal
