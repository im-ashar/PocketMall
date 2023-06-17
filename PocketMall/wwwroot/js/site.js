const connection = new signalR.HubConnectionBuilder()
    .withUrl("/connection") // Replace with the actual URL of your SignalR hub
    .build();

connection.start().then(() => { }).catch((error) => {
    console.error(error);
});


var notifier = new AWN();
connection.on("ProductAdded", (order) => {
    console.log(order);
    notifier.success(order)
});

connection.on("AddedToCart", (order) => {
    console.log(order);
    notifier.success(order)
});

//For Searching Category
$('.selectpicker').selectpicker();