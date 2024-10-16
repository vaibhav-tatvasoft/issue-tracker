$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    $('#tblData').DataTable({
        "ajax": { url: '/admin/product/getall', type: 'GET' },
        "columns": [
            { data: 'title', "width": "15%" },
            { data: 'isbn', "width": "15%" },
            { data: 'author', "width": "15%" },
            { data: 'price', "width": "10%" },
            { data: 'category.name', "width": "15%" },
            {
                data: 'id',
                render: function (data) {
                    return `
        <div class="p-2"> <!-- Add padding here -->
            <div class="d-flex justify-content-between w-100">
                <a href="/Admin/Product/Upsert?id=${data}" class="btn btn-sm btn-primary w-100 me-1">Edit</a>
                <a href="/Admin/Product/Delete?id=${data}" class="btn btn-sm btn-danger w-100 me-1">Delete</a>
            </div>
        </div>
    `;
                },
            }
        ]
    });
}
