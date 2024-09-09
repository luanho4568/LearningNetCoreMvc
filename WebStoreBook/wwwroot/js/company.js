var dataTable;

$(document).ready(function () {
    loadDataTable()
})

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/admin/company/getall"
        },
        "columns": [
            { "data": "name", "width": "calc(100%/7)" },
            { "data": "streetAddress", "width": "calc(100%/7)" },
            { "data": "city", "width": "calc(100%/7)" },
            { "data": "state", "width": "calc(100%/7)" },
            { "data": "postalCode", "width": "calc(100%/7)" },
            { "data": "phoneNumber", "width": "calc(100%/7)" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div>
                            <a href="/Admin/Company/Upsert?id=${data}" class="btn btn-warning">
                                <i class="bi bi-pencil-square"></i>
                            </a>
                            <a class="btn btn-danger" onclick="return Delete('/Admin/Company/Delete?id=${data}')">
                                 <i class="bi bi-trash"></i>
                            </a>

                        </div>
                    `
                },
                "width": "calc(100%/7)"
            },
        ]
    })
}
function Delete(url) {
    Swal.fire({
        title: "Bạn có chắc muốn xoá sản phẩm này không?",
        text: "Không thể khôi phục sau khi xoá!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                },
                error: function () {
                    toastr.error("Có lỗi xảy ra khi xóa sản phẩm.");
                }
            });
        }
    });
}
