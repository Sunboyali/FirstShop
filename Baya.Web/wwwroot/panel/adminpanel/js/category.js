



$('#edit_button').on("click", function () {

    var Id = $('#jstree').jstree("get_selected");
    if (Id != 0) {
        window.location.href = '/Admin/EditCategory/' + Id;
       
    } else {
        swal("ابتدا یک دسته بندی را انتخاب کنید");
        return false;
    }

});

$('#delete_button').on("click", function () {
    var id = $('#jstree').jstree("get_selected");


    if (id != 0) {
        $.ajax({
            type: "Get",
            url: '/AdminPanel/Category/Delete/' + id,
            cache: false,
            data: {}

        }).done(function (msg) {
            if (msg.message === "havesubcategory") {
                swal("این دسته بندی دارای زیر دسته است ابتدا زیر دسته را حذف کنید");
                return false;
            } else if (msg.message === "success") {
                swal({
                    title: "تأیید حذف",
                    text: "آیا از حذف این مقدار اطمینان دارید؟",
                    icon: "warning",
                    buttons: ['لغو', 'بلی'],
                    dangerMode: true,
                })
                    .then((willDelete) => {
                        if (willDelete) {
                            window.open('/AdminPanel/Category/DeleteCat/' + id, '_parent');
                        }
                    });
            }


        });
    } else {
        swal("ابتدا یک دسته بندی را انتخاب کنید");
        return false;
    }

});