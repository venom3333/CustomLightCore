// Запуск карусели
$('.carousel').carousel({
    interval: 5000 //changes the speed
});

$(function() {
    $(document).ready(function() {
        callBackMail();
    });
});

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
        }

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

