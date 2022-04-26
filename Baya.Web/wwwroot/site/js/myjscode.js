
// searchResult--------------------------------------
var app = angular.module('app', []);
app.controller('Search', function ($scope) {
    let search = this;
    search.Search_Id = '';

    var Search_Input = $('.search-input');
    var Search_Result = $('.search-result');

    search.check_search = function () {
        if (search.Search_Id == 0) {
            $(Search_Input).parents('.search-header').addClass('show-result').find(Search_Result).fadeIn();
            $('.search-overlay').addClass('opened');
            $('#del_input_search').hide();
            //$('.slide__product_search').hide();
            $('.category_search_result').hide();
            $('ul.search-result-list').show();
        } else {

            //Search-Ajax
            $.ajax({
                url: "/Home/SearchResult",
                type: 'post',
                data: { "txtsearch": $("#searchTerm").val() },
                success: function (data) {
                    $('#popup').html(data);
                    $('#txt__searched').text(search.Search_Id);
                }
            });

            $('ul.search-result-list').hide();
            $('.search-overlay').addClass('opened');
            $('#del_input_search').show();
            //$('.slide__product_search').show();
            $('.category_search_result').show();
        }
    };
    search.ShowResult = function () {
        $('.search-input').parents('.search-header').addClass('show-result').find('.search-result').fadeIn();
        $('.search-overlay').addClass('opened');
        $('ul.search-result-list').show();
        if ($('.search-input').val() == null) {
            $('.category_search_result').hide();
        }
        //$('.slide__product_search').hide();



    };

    search.Clear_Text_Input = function () {
        $('#searchTerm').val('');
        $('#del_input_search').hide();
        //$('.slide__product_search').hide();
        $('.category_search_result').hide();
        $('ul.search-result-list').show();
    };

});
$(document).click(function (e) {
    if ($(e.target).is('.search-header *')) return;
    //$('#searchTerm').val('');
    $('.search-result').hide();
    $('.search-overlay').removeClass('opened');
    $('#del_input_search').hide();
});

$(function () {
    function getItems(key) {
        //console.log("Getting Items from key: " + key);
        var results = [];
        try {
            results = localStorage.getItem(key).split(',');
        } catch (e) {
            //console.log(e);
        }
        //console.log(results);
        return results;
    }

    function setItems(key, data) {
        //console.log("Setting Items into key: " + key);
        try {
            localStorage.setItem(key, data.join(','));
            return true;
        } catch (error) {
            //console.log(error);
            return false;
        }
    }

    function resetItems(key) {
        localStorage.removeItem(key);
        $("#searchStory").empty();
    }

    function searchItems(key, term) {
        //console.log("Search: " + key + ", for: " + term);
        var items = getItems(key);
        if (items.indexOf(term) == -1) {
            //console.log("Not Found. Pushing it.");
            items.push(term);
        }
        items.sort();
        //console.log(items);
        return items;
    }

    function showSearchStory(event) {
        if (event != undefined) {
            event.preventDefault();
        }
        let k = "Search_Text";
        let t = $("#searchTerm").val();
        var vals;
        //console.log("Trigger Search for: " + t);
        if (k && t != undefined && t.length > 0) {
            if (getItems(k).length == 0) {
                setItems(k, [t]);
            }
            vals = searchItems(k, t);
            setItems(k, vals);
            $("#searchStory").empty();
            $.each(vals, function (i, s) {
                $("<a>", {
                    name: "search_txt",
                    class: "txt-search-story",
                    href: "#"
                }).html("<span>" + s + "</span>").appendTo($("#searchStory"));
            });
            //location.reload();
        } else {
            vals = getItems(k);
            $.each(vals, function (i, s) {
                $("<a>", {
                    name: "search_txt",
                    class: "txt-search-story",
                    href: "#"
                }).html("<span>" + s + "</span>").appendTo($("#searchStory"));
            });
        }
        return false;


    }

    $("#searchForm").submit(function () {

        showSearchStory();
    });


    $("#clearStory").click(function () {
        resetItems("Search_Text");
    });

    showSearchStory();
    $('.txt-search-story').click(function () {
        var txtspan = $(this).find('span').text();
        $.ajax({

            url: "/Product/GetTextSearch",
            type: 'Get',
            data: { "q": txtspan },
            success: function (response) {
                window.location.href = response.redirectToUrl;
            }
        });
    });

});

// searchResult--------------------------------------

//  Cart Toggle Js

$(document).ready(function () {
    $(".cart-toggle-btn").on("click", function () {
        $(".cartmini__wrapper").addClass("opened");
        $(".body-overlay").addClass("opened");
    });
    $(".cartmini__close-btn").on("click", function () {
        $(".cartmini__wrapper").removeClass("opened");
        $(".body-overlay").removeClass("opened");
    });
    $(".body-overlay").on("click", function () {
        $(".cartmini__wrapper").removeClass("opened");
        //$(".sidebar__area").removeClass("sidebar-opened");
        $(".body-overlay").removeClass("opened");
    });






    //  User Toggle Js

    $('.user-toggle-btn').click(function () {
        $('.box__info').fadeIn(500);
    });
    $('#img-close').click(function () {
        $('.box__info').fadeOut(500);
    });


    //$(".user-toggle-btn").on("click", function () {
    //	$(".usercartmini__wrapper").addClass("opened");
    //	$(".body-overlay").addClass("opened");
    //});
    //$(".cartmini__close-btn").on("click", function () {
    //	$(".usercartmini__wrapper").removeClass("opened");
    //	$(".body-overlay").removeClass("opened");
    //});
    //$(".body-overlay").on("click", function () {
    //	$(".usercartmini__wrapper").removeClass("opened");
    //	//$(".sidebar__area").removeClass("sidebar-opened");
    //	$(".body-overlay").removeClass("opened");
    //});



    //  Favorite Toggle Js
    $(".fav-toggle-btn").on("click", function () {
        $(".favoritecartmini__wrapper").addClass("opened");
        $(".body-overlay").addClass("opened");
    });
    $(".cartmini__close-btn").on("click", function () {
        $(".favoritecartmini__wrapper").removeClass("opened");
        $(".body-overlay").removeClass("opened");
    });
    $(".body-overlay").on("click", function () {
        $(".favoritecartmini__wrapper").removeClass("opened");
        //$(".sidebar__area").removeClass("sidebar-opened");
        $(".body-overlay").removeClass("opened");
    });



    //  Cart Plus Minus Js
    $('.cart-minus').click(function () {
        var $input = $(this).parent().find('input');
        var count = parseInt($input.val()) - 1;
        count = count < 1 ? 1 : count;
        $input.val(count);
        $input.change();
        return false;
    });
    $('.cart-plus').click(function () {
        var $input = $(this).parent().find('input');
        $input.val(parseInt($input.val()) + 1);
        $input.change();
        return false;
    });

});


//------------------ Category--------------------------------------
$(document).ready(function () {
    //   start Menu Fixed---------------------------

    $(document).scroll(function () {
        var scroll = $(document).scrollTop();
        if (scroll > 200) {
            $('nav.main-menu,.sidebar,nav-slider').addClass("fixed-menu");
        } else if (scroll < 150) {
            $('nav.main-menu,.sidebar,nav-slider').removeClass("fixed-menu");
        }
    });



    // categorylist----------------------------------
    $('.main-menu .sub-menu .list-item-children:first-child').addClass('active-megamunu');
    $('.main-menu .sub-menu .list-item-children').on('mouseenter', function () {
        $(this).addClass('active-megamunu').siblings().removeClass('active-megamunu');
    });
    // categorylist----------------------------------


    //    hover-menu-overlay-------------------------
    $('li.nav-overlay').hover(function () {
        //$('.sub-menu').removeClass('active');
        $('.nav-categories-overlay').addClass('active');
    }, function () {
        $('.nav-categories-overlay').removeClass('active');
    });
    //    hover-menu-overlay-------------------------



    //    resposive-megamenu-mobile------------------   

    $(document).on('click', function (e) {
        if ($('.dropdown').hasClass('open')) {
            $('.dropdown').removeClass('open');
        }
    });

    $('.nav-btn.nav-slider').on('click', function () {
        $('.overlay').show();
        $('nav').toggleClass("open");
    });

    $('.overlay').on('click', function () {
        if ($('nav').hasClass('open')) {
            $('nav').removeClass('open');
        }
        $(this).hide();
    });


    //$('li.active').addClass('open').children('ul').show();


    $("li.has-sub > a").on('click', function () {
        $(this).removeAttr('href');
        var e = $(this).parent('li');
        if (e.hasClass('open')) {
            e.removeClass('open');
            e.find('li').removeClass('open');
            e.find('ul').slideUp(200);
        }
        else {
            e.addClass('open');
            e.children('ul').slideDown(200);
            e.siblings('li').children('ul').slideUp(200);
            e.siblings('li').removeClass('open');
            e.siblings('li').find('li').removeClass('open');
            e.siblings('li').find('ul').slideUp(200);
        }
    });



    $("li.has-sub-second > a").on('click', function () {
        $(this).removeAttr('href');
        var e = $(this).parent('li');
        if (e.hasClass('open')) {
            e.removeClass('open');
            e.find('li').removeClass('open');
            e.find('ul').slideUp(200);
        }
        else {
            e.addClass('open');
            e.children('ul').slideDown(200);
            e.siblings('li').children('ul').slideUp(200);
            e.siblings('li').removeClass('open');
            e.siblings('li').find('li').removeClass('open');
            e.siblings('li').find('ul').slideUp(200);
        }
    });


//    resposive-megamenu-mobile------------------
});


