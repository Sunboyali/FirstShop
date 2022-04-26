const btnSendActive = document.getElementsByClassName('Send_Active');

function Alert(content) {
    $.alert({
        title: 'خطا',
        content: content,
        rtl: true,
        closeIcon: true,
        type: 'red',
        typeAnimated: true,
        buttons: {
            confirm: {
                text: 'تایید',
                btnClass: 'btn-red',
            }

        },
    });
};


function Alert_Success(content) {
    $.alert({
        title: 'توجه',
        content: content,
        rtl: true,
        closeIcon: true,
        type: 'green',
        typeAnimated: true,
        buttons: {
            confirm: {
                text: 'تایید',
                btnClass: 'btn-success',
            }
        },
    });
};
const key_Cl = "cl2154615dasdgsad656";


function buttonClicked() {

    const User__Mobile = $('.user__mobile').val();
    btnSendActive[0].removeEventListener('click', buttonClicked);
    btnSendActive[1].removeEventListener('click', buttonClicked);
    enableButton();
    var currentValue = localStorage.getItem(key_Cl) ? parseInt(localStorage.getItem(key_Cl)) : 0;
    var newValue = currentValue + 1;
    localStorage.setItem(key_Cl, newValue);

    const Get_Last = localStorage.getItem(key_Cl);
    if (Get_Last > 3) {
        Alert('شما بیشتر از 3 بار نمی توانید تقاضای کد فعالسازی کنید در صورت عدم دریافت پیامک به مدیر سایت اطلاع دهید');
        return false;
    } else {
        $.post("/Account/SendAgainActiveCode", {
            Mobile: User__Mobile
        }, function (data) {
            if (data == 0) {
                Alert('شماره موبایل در سیستم یافت نشد');
                return false;
            } else {
                Alert_Success('کد فعالسازی به شماره موبایل شما ارسال گردید');
                return false;
            }

        });
    }

}


function enableButton() {
    let timer2 = "00:10";
    let interval = setInterval(function () {
        var timer = timer2.split(':');
        var minutes = parseInt(timer[0], 10);
        var seconds = parseInt(timer[1], 10);
        --seconds;
        minutes = (seconds < 0) ? --minutes : minutes;
        if (minutes < 0) clearInterval(interval);
        seconds = (seconds < 0) ? 59 : seconds;

        $('.countdown').html(minutes + ':' + seconds);
        timer2 = minutes + ':' + seconds;
        if (minutes == 0 && seconds == 0) {
            clearInterval(interval);
            $('.Send_Active').prop('hidden', false);
            $('.return-from-mobile').prop('disabled', false);
            btnSendActive[0].addEventListener('click', buttonClicked);
            btnSendActive[1].addEventListener('click', buttonClicked);
        } else {
            $('.return-from-mobile').prop('disabled', true);
            $('.Send_Active').prop('hidden', true);
        }
    }, 1000);

}



const Mobile_Form = document.getElementById('Mobile-Form');
const Register_Form = document.getElementById('Register-Form');
const Login_Form = document.getElementById('Login-Form');
const Input_Mobile = document.getElementById('login-input-mobile');
let User_Mobile;
const True_Mobile = /^09\d{9}/;
const Arregexe = /[0-9\-\(\)\s]+/;
const Status_Loading = document.getElementById('status_Mobile');

function Validate_Mobile() {

    User_Mobile = Input_Mobile.value;

    if (User_Mobile == '') {
        Input_Mobile.focus();
        Alert('لطفا شماره موبایل  را وارد نمایید');
        return false;

    } else if (!True_Mobile.test(User_Mobile) || User_Mobile.length > 11) {
        Input_Mobile.focus();
        Alert('فرمت شماره موبایل صحیح نمی باشد');
        return false;
    }
    //Send Active Code
    $.post("/Account/SendMobile", {
        Mobile: User_Mobile
    }, function (data) {
        if (data == 3) {
            Alert('لطفا شماره موبایل  را وارد نمایید');
            return false;
        } else if (data == 0) {//Register
            enableButton();
            Status_Loading.removeAttribute('hidden');
            Mobile_Form.setAttribute('hidden', true);
            Register_Form.removeAttribute('hidden');
            Input_Mobile.setAttribute('disabled', true);
            $('#user__mobile').val(User_Mobile);

        } else if (data == 1) {//Login            
            Status_Loading.removeAttribute('hidden');
            Mobile_Form.setAttribute('hidden', true);
            Input_Mobile.setAttribute('disabled', true);
            Login_Form.removeAttribute('hidden');
            $('#mobile_User').val(User_Mobile);
            $('#forget-input-mobile').val(User_Mobile);

        } else {
            Alert('متاسفانه شما توسط مدیر مسدود شده اید و اجازه ورود به سایت را ندارید.لطفا از طریق تماس با ما به مدیر سایت اعلام کنید');
        }

    });
    return false;
}


function Validate_Register() {
    User_Mobile = Input_Mobile.value;

    let Register_Password_Value = $('#Register-NewPassword').val();

    let Register_Active_Value = $('#Register-ActiveCode').val();

    const Status_Loading = $('#status_Register');
    let Length_Register_Active = Register_Active_Value.length;
    let Length_Register_Password = Register_Password_Value.length;
    if (Length_Register_Password < 6) {
        $('#Register-NewPassword').focus();
        Alert('رمز عبور جدید باید حداقل 6 کاراکتر باشد');
        return false;
    }
    if (Length_Register_Active != 6) {
        $('#Register-ActiveCode').focus();
        Alert('کد فعالسازی نمی تواند کمتر یا بیشتر از 6 کاراکتر باشد');
        return false;
    }

    if (!Arregexe.test(Register_Active_Value)) {
        $('#Register-ActiveCode').focus();
        Alert('لطفا فقط عدد وارد نمایید');
        return false;
    }
    $.post("/Account/CheckActiveCode", {
        Mobile: User_Mobile, ActiveCode: Register_Active_Value
    }, function (data) {
        if (data == 1) {
            Alert('کد فعالسازی وارد شده صحیح نمی باشد');
            return false;
        } else {
            Status_Loading.prop("hidden", true);
            $("#ReisterForm").submit();
            localStorage.removeItem(key_Cl);
        }

    });

    return true;
}


function Validate_Login() {
    User_Mobile = Input_Mobile.value;
    let Login_Password_User = $('#Login-Password').val();
    const Status_Loading = $('#status_Password');
    Status_Loading.prop("hidden", true);

    if (Login_Password_User == '') {
        $('#Login-Password').focus();
        Alert('لطفا رمز عبور خود  را وارد نمایید');
        return false;
    }

    $.post("/Account/CheckExistUser", {
        Mobile: User_Mobile, Password: Login_Password_User
    }, function (data) {

        if (data == 1) {
            Alert('شماره موبایل یا رمز عبور شما اشتباه است');
            return false;
        } else {
            Status_Loading.prop("hidden", false);
            $("#LoginForm").submit();
        }
    });
    return true;
}

function return_Mobile() {

    Register_Form.setAttribute('hidden', true);
    Mobile_Form.removeAttribute('hidden');
    Input_Mobile.removeAttribute('disabled');
    Status_Loading.setAttribute('hidden', true);

}
function return_Mobile_Login() {
    $('.return-from-mobile').click(function () {
        Login_Form.setAttribute('hidden', true);

        Mobile_Form.removeAttribute('hidden');
        Input_Mobile.removeAttribute('disabled');
        Status_Loading.setAttribute('hidden', true);
    });
}

//////////////////////////Forget//////////////////////////////////////////////

function Validate__Mobile() {
    const Login_Mobile_Input = $('#forget-input-mobile');
    let Login_Mobile = Login_Mobile_Input.val();
    const Status_Loading = $('#status__Mobile');

    Status_Loading.prop("hidden", true);

    if (Login_Mobile == '') {
        Login_Mobile_Input.focus();
        Alert('لطفا شماره موبایل  را وارد نمایید');
        return false;

    } else if (!True_Mobile.test(Login_Mobile) || Login_Mobile.length > 11) {
        Login_Mobile_Input.focus();
        Alert('فرمت شماره موبایل صحیح نمی باشد');
        return false;
    }
    //Send Active Code
    $.post("/Account/SendMobileToForget", {
        Mobile: Login_Mobile
    }, function (data) {
        if (data == 0) {
            Alert('این شماره موبایل در سیستم ثبت نشده است لطفا ثبت نام کنید');
            return false;
        } else if (data == 1) {
            Alert('لطفا شماره موبایل  را وارد نمایید');
            return false;
        } else if (data == 2) {
            Alert('متاسفانه این شماره موبایل توسط مدیر سایت مسدود شده است و اجازه ورود به سایت را ندارد.لطفا از طریق تماس با ما به مدیر سایت اعلام کنید');

        } else { //Change Pass 
            enableButton();
            Status_Loading.prop("hidden", false);
            $('#Change-Pass-Form').prop("hidden", false);
            Login_Mobile_Input.prop("disabled", true);
            $('#Mobile-Form-Chpass').prop("hidden", true);
            $('.user__mobile').val(Login_Mobile);
        }

    });
    return false;
}

function Validate_Ch__Pass() {
    const Forget_Mobile_Input = $('#forget-input-mobile').val();
    let New_Password_Value = $('#New__Password').val();
    let Forget_Active_Value = $('#Forget__ActiveCode').val();
    let Length_New_Password = New_Password_Value.length;
    let Length_Forget_Active = Forget_Active_Value.length;
    const Status_Loading = $('#status_NewPass');
    if (Length_Forget_Active != 6) {
        $('#Forget__ActiveCode').focus();
        Alert('کد فعالسازی نمی تواند کمتر یا بیشتر از 6 کاراکتر باشد');
        return false;
    }

    if (!Arregexe.test(Forget_Active_Value)) {
        $('#Forget__ActiveCode').focus();
        Alert('لطفا فقط عدد وارد نمایید');
        return false;
    }
    if (Length_New_Password < 6) {
        $('#New__Password').focus();
        Alert('رمز عبور جدید باید حداقل 6 کاراکتر باشد');
        return false;
    }

    $.post("/Account/CheckActiveCode", {
        Mobile: Forget_Mobile_Input, ActiveCode: Forget_Active_Value
    }, function (data) {
        if (data == 1) {
            Alert('کد فعالسازی وارد شده صحیح نمی باشد');
            return false;
        } else {
            Status_Loading.prop("hidden", true);
            $("#ChangePassForm").submit();
            localStorage.removeItem(key_Cl);
        }

    });

    return true;
}