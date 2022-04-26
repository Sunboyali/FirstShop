
var Reg_Persian = /^[\u0600-\u06FF\s]+$/;
var Reg_English = /^[A-Za-z]*$/;
var Reg_Number = /[0-9\-\(\)\s]+/;
var Just_Number = /^\d+$/;
var TrueEmail = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/

/*var True_Mobile = /^09\d{9}/;*/
var Format_Date = /^[1-4]\d{3}\/((0[1-6]\/((3[0-1])|([1-2][0-9])|(0[1-9])))|((1[0-2]|(0[7-9]))\/(30|([1-2][0-9])|(0[1-9]))))$/;


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

// ---------------------------------swiper MainSlider----------------------------
var swiper = new Swiper('.swiper-introIndex1', {
    slidesPerView: 1,
    spaceBetween: 30,
    loop: true,

    pagination: {
        el: '.swiper-pagination',
        clickable: true,
    },

    autoplay:
    {
        delay: 3000,
    },
    keyboard:
    {
        enabled: true,
        onlyInViewport: false,
    },
    navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
    },
});
// -

// ---------------------------------swiper - bannerindex2----------------------------
var swiper = new Swiper('.swiper-introIndex2', {
    slidesPerView: 1,
    spaceBetween: 30,
    loop: true,

    pagination: {
        el: '.swiper-pagination',
        clickable: true,
    },

    autoplay:
    {
        delay: 7000,
    },
    keyboard:
    {
        enabled: true,
        onlyInViewport: false,
    },
    navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
    },
});

// ---------------------------------swiper - bannerindex3----------------------------
var swiper = new Swiper('.swiper-introIndex3', {
    slidesPerView: 1,
    spaceBetween: 30,
    loop: true,

    pagination: {
        el: '.swiper-pagination',
        clickable: true,
    },

    autoplay:
    {
        delay: 4500,
    },
    keyboard:
    {
        enabled: true,
        onlyInViewport: false,
    },
    navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
    },
});
// -





// --------------------------------Show Fast Product----------------------------
function ShowFastProduct(id) {
    $.ajax({
        url: "/Product/ShowFastProduct/" + id,
        type: "Get",
        data: {}
    }).done(function (result) {
        $('#productModal').modal('show');
        $('#bodyModal').html(result);
    });
}

// --------------------------------Add Favorite Product----------------------------

function addfavorite(id) {
    $.ajax({
        type: "post",
        url: "/Cart/AddFavorite/" + id,
        data: {},
    }).done(function (msg) {
        if (msg.status === 'notlogin') {
            toastr.info('ابتدا باید وارد حساب کاربری خود شوید');
            return false;
        } else if (msg.status === 'added') {
            $('#favarite-user').load(' #favarite-user');
            const itemsfav = $("#items-fav");
            itemsfav.text(msg.result);
            itemsfav.css("visibility", "visible");

            $(`.product-card-button ul li div a[data-addfav=${id}]`).addClass('selected-fav');
            $(`.product-card-button ul li div a[data-addfav=${id}]`).remove('p');
            toastr.success('به لیست علاقه مندی ها اضافه شد');
        } else {
            toastr.error('این کالا در لیست شما وجود دارد');
        };

    });
};

// --------------------------------Remove Favorite Product----------------------------

function deletefavorite(id) {

    $.ajax({
        type: "post",
        url: "/Cart/DeleteFavorite/" + id,
        data: {}
    }).done(function (msg) {
        if (msg.status === 'success') {
            $(`.fav-basket[data-favorite-li=${id}]`).remove();
            if (msg.result == 0) {
                $("#no-fav").css("visibility", "visible");
                $("#items-fav").css("visibility", "hidden");
            }
            $("#items-fav").text(msg.result);
        }
    });
}

// --------------------------------Add Basket Product----------------------------
function addBasket(id) {
    $.ajax({
        type: "post",
        url: "/Cart/AddCard/" + id,
        data: {},
    }).done(function (msg) {
        if (msg.status === 'notlogin') {
            toastr.info('ابتدا باید وارد حساب کاربری خود شوید');
            return false;
        } else {
            $('#basket-user').load(' #basket-user');
            let items = $("#items-bag");
           

            items.text(msg.result);


            items.css("visibility", "visible");
           
            
            toastr.success('به سبد خرید اضافه شد');
        } 
           
        

    });
};

// --------------------------------Remove Basket Product----------------------------
function deletecartpro(id) {
    $.ajax({
        type: "post",
        url: "/Cart/DeleteBasketProduct/" + id,
        data: {}
    }).done(function (msg) {
        if (msg.status === 'success') {
            $(`.product-basket[data-OrderDetailId=${id}]`).remove();
            $('#total-price').text(msg.result);
            if (msg.result2 == 0) {
                $('.cartmini__checkout').hide();
                $("#no-product").css("visibility", "visible");
                $("#items-bag").css("visibility", "hidden");
            } else {
                $("#items-bag").text(msg.result2);

            }
        }

    });
}


