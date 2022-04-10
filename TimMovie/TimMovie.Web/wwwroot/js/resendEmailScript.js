function resendEmail(){
    console.log('clicked');
    let email = $("#emailInput").val();
    $.ajax({
        type: "POST",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        url:"/Account/SendConfirmEmail",
        data:{email:email},
        success: function (){
            console.log('resend email');
        },
        error: function () {
            console.log('error occured')
        }
    })
}