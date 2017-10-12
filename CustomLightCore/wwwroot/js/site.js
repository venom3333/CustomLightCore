// Запуск карусели
$('.carousel').carousel({
    interval: 5000 //changes the speed
});

$(function () {
    $(document).ready(function () {
        // Перезвоните мне
        callBackMail();

        // Оформить заказ
        orderMail();
    });

    lightbox.option({
        'resizeDuration': 200,
        'wrapAround': true
        //'disableScrolling': true
    });
});

// Функция оформить заказ
function orderMail() {
    $(".order-mail-button").click(function () {
        var name = $("#orderName").val();
        var phone = $("#orderPhone").val();
        if (name === "" || phone === "") {
            alertify.error("Введите Имя и телефон!");
            return;
        }

        var form = $("#orderForm");

        var model = form.serialize();

        $.ajax({
            type: "POST",
            url: "/Mail/OrderMail",
            data: model,
            success: function (html) {
                //console.log(html);
                $('#orderForm').html(html);
                alertify.success("Ваш заказ отправлен. Скоро мы Вам перезвоним.");
            },
            error: function (msg) {
                console.log(msg);
            }
        });
    });
}

// Функция перезвоните мне
function callBackMail() {
    $(".callback-mail-button").click(function () {
        var name = $("#callBackName").val();
        var phone = $("#callBackPhone").val();
        var misc = $("#callBackMisc").val();

        if (name === "" || phone === "") {
            alertify.error("Введите Имя и телефон!");
            return;
        }

        var model = {
            Name: name,
            Phone: phone,
            Misc: misc
        };

        $.ajax({
            type: "POST",
            url: "/Mail/CallBackMail",
            data: model,
            success: function (html) {
                //console.log(html);
                $('#callBackMail').html(html);
                alertify.success("Сообщение отправлено. Скоро мы Вам перезвоним.");
            },
            error: function (msg) {
                console.log(msg);
            }
        });
    });
}

