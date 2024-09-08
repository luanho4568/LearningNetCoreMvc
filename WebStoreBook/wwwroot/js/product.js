var dataTable;

$(document).ready(function () {
    loadDataTable()
})

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/admin/product/getall"
        },
        "columns": [
            { "data": "title", "width": "15%" },
            { "data": "isbn", "width": "15%" },
            { "data": "author", "width": "15%" },
            { "data": "category.name", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div>
                            <a href="/Admin/Product/Upsert?id=${data}" class="btn btn-warning">
                                <i class="bi bi-pencil-square"></i>
                            </a>
                            <a class="btn btn-danger" onclick="return Delete('/Admin/Product/Delete?id=${data}')">
                                 <i class="bi bi-trash"></i>
                            </a>

                        </div>
                    `
                },
                "width": "15%"
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
