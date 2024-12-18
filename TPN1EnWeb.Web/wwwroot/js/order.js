var dataTable;
$(function () {
    loadDataTable(); //Esta es la función que se va a ejecutar cuando se cargue el DOM
});

function loadDataTable() { //En esta función le indico que se llene la tabla
    dataTable = new DataTable('#orderTable', { //Este es el id que le di en el index  y el # es como se inicializa la tabla que traigo de la pág DataTable
        "ajax": {
            "url": "/Customer/Orders/GetAll" //En esta url me traigo los datos
        },
        "columns": [ //Creo las columnas y como son varias las pongo dentro de un Array
            { "data": "orderHeaderId" },//Se utiliza la minuscula en el nombre de la columna ya que así es como es la convención en JavaScript                         //y porque así viene serializado en Json
            {
                "data": "orderDate",
                "render": function (data) {
                    return moment(data).format('DD/MM/YYYY');
                }
            },
            {
                "data": "shippingDate",
                "render": function (data) {
                    return moment(data).format('DD/MM/YYYY');
                }
            },
            {
                "data": "orderTotal",
                "render": function (data) {
                    return numeral(data).format('$0,0.00');
                }
            },
            {
                "data": "orderHeaderId",
                "render": function (data) {                    //Le indico la ruta por hacía donde tiene que ir con un href ya que el asp acá no funciona debido a que es JavaScript y a través del data le paso el id del OrderHeader
                    return `
                             <a class="btn btn-info" href="/Customer/Orders/Details?id=${data}"> 
                                <i class="bi bi-card-list"></i>&nbsp;
                                Details
                            </a>

                    `
                }
            }

        ]
    });
}