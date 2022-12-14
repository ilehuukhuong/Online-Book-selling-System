$(document).ready(function () {
    ShowCount();
    $('body').on('click', '.btnAddToCart', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var quatity = 1;
        var tQuantity = $('quantity_value').text();
        if (tQuantity != '') {
            quatity = parseInt(tQuantity);
        }
        //alert(id + " " + quatity);
        $.ajax({
            url: '/shoppingcart/addtocart',
            type: 'POST',
            data: { id: id, quantity: quatity },
            success: function (rs) {
                if (rs.Success) {
                    $('#checkout_items').html(rs.Count);
                    alert(rs.msg);
                }
            }
        });
    });
    

    $('body').on('click', '.btnAddToCartDetail', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var quatity = 1;
        var tQuantity = document.getElementById("quantity_value").innerHTML;
        if (tQuantity != '') {
            quatity = parseInt(tQuantity);
        }
        $.ajax({
            url: '/shoppingcart/addtocart',
            type: 'POST',
            data: { id: id, quantity: quatity },
            success: function (rs) {
                if (rs.Success) {
                    $('#checkout_items').html(rs.Count);
                    alert(rs.msg);
                }
            }
        });     
    });
    $('body').on('click', '.btnDelete', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var quatity = 1;
        var tQuantity = $('quantity_value').text();
        if (tQuantity != '') {
            quatity = parseInt(tQuantity);
        }
        var conf = confirm('Are you sure to delete product?')
        if (conf == true) {
            $.ajax({
                url: '/shoppingcart/Delete',
                type: 'POST',
                data: { id: id },
                success: function (rs) {
                    if (rs.Success) {
                        $('#checkout_items').html(rs.Count);
                        $('#trow_' + id).remove();
                    }
                }
            });
        }
    });
});
function ShowCount() {
    $.ajax({
        url: '/shoppingcart/ShowCount',
        type: 'GET',
        success: function (rs) {
            $('#checkout_items').html(rs.Count);
        }
    });
}