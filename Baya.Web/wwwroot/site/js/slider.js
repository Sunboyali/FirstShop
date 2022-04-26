
$(document).ready(function () {
    // -----------------Slider-Search--------------------------------------

    const SliderSearch = $('.slide__product_search');
    const SliderSearchUl = SliderSearch.find('ul.slide_items');
    const SliderSearchItems = SliderSearchUl.find('li');
    let Slider_Numbers = SliderSearchItems.length;
    SliderSearchUl.css('width', Slider_Numbers * 250);

    let SliderSearchItemsCounts = SliderSearchItems.length;
    var Slide_Number = Math.round(SliderSearchItemsCounts / 2);

    var Max_Margin = -(Slide_Number - 1) * 500; 

    const Prev_Botton = ('.slide__product_search .button-prev');
    const Next_Botton = ('.slide__product_search .button-next');
    let Right_Margin = parseFloat(SliderSearchUl.css('margin-right'));

    if (Right_Margin == 0) {
        $(Prev_Botton).hide();
    }
    function Slider_Search(direction) {
        let Right_Margin = parseFloat(SliderSearchUl.css('margin-right'));  

        var Margin_New;

        if (direction == 'next') {
            Margin_New = Right_Margin - 250;
            $(Prev_Botton).show();

        }

        if (direction == 'prev') {
            Margin_New = Right_Margin + 250;
            if (Margin_New == 0) {
                $(Prev_Botton).hide();
            }
            if (Margin_New > Max_Margin) {
                $(Next_Botton).show();
            }      

        }
        if (Margin_New <= Max_Margin) {
            $(Next_Botton).hide();
        } 

        SliderSearchUl.animate({ 'marginRight': Margin_New }, 300);
    }

    $(Next_Botton).click(function () {
        Slider_Search('next');
    });

    $(Prev_Botton).click(function () {
        Slider_Search('prev');
    });
});