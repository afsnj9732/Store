document.addEventListener('DOMContentLoaded', function () {
    GetCartItemQuantity();
    PostAntiToken();
});
function GetCartItemQuantity() {
    var token = document.querySelector('input[name="__RequestVerificationToken"]').value;
    var data = { __RequestVerificationToken: token }
    $.ajax({
        url: "../Shopping/GetCartItemQuantity",
        type: "POST",
        dataType: "json",
        data:data,
        success: function (data) {
            if (data.ajaxStatus != 401) {
                if (data.CartItemQuantity != null) {
                    document.getElementById("cartItemsQuantity").textContent = data.CartItemQuantity;
                } else {
                    document.getElementById("cartItemsQuantity").textContent = 0;
                }
            }
        },
        error: function (xhr, status, error) {
            console.error("ajax error")
        }
    });
    //fetch(url,
    //    {
    //        method: "POST",
    //        headers: {
    //            'Content-Type': 'application/json' 
    //        },
    //    }
    //)
    //    .then(Response => {
    //        return Response.json();
    //    })
    //    .then(data => {
    //        if (data.ajaxStatus != 401) {
    //            if (data.CartItemQuantity != null) {
    //                document.getElementById("cartItemsQuantity").textContent = data.CartItemQuantity;
    //            } else {
    //                document.getElementById("cartItemsQuantity").textContent = 0;
    //            }
    //        }

    //    })
}
function AddCart(listID) {
    var token = document.querySelector('input[name="__RequestVerificationToken"]').value;
    var data = {
        productID: listID,
        __RequestVerificationToken: token    }
    $.ajax({
        url: "../Shopping/AddCart",
        type: "POST",
        dataType: "json",
        data: data,
        success: function (data) {
            if (data.ajaxStatus == 401) {
                window.location.href = '../Validate/Login';
            } else {
                GetCartItemQuantity();
            }
        },
        error: function (xhr, status, error) {
            console.error("ajax error")
        }
    });
    //fetch(url,
    //    {
    //        method: "POST",
    //        headers: {
    //            'Content-Type': 'application/json'
    //        },
    //        body: JSON.stringify(data)
    //    }
    //)
    //    .then(Response => {
    //        return Response.json();
    //    })
    //    .then(data => {
    //        if (data.ajaxStatus == 401) {
    //            window.location.href = '../Validate/Login'
    //        } else {
    //            GetCartItemQuantity();

    //        }
    //    })

}

function PostAntiToken() {
    var token = document.querySelector('input[name="__RequestVerificationToken"]').value;
    var form = document.querySelector('form');   
    if (form != null) {
    var formTokenInput = document.getElementById('AntiToken');
    formTokenInput.value = token;
    }

}


