$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    $('#tblData').DataTable({
        "ajax": { url: '/admin/company/getall', type: 'GET' },
        "columns": [
            { data: 'name', "width": "15%" },
            { data: 'streetAddress', "width": "15%" },
            { data: 'city', "width": "15%" },
            { data: 'state', "width": "10%" },
            { data: 'phoneNumber', "width": "15%" },
            {
                data: 'id',
                render: function (data) {
                    return `
        <div class="p-2"> <!-- Add padding here -->
            <div class="d-flex justify-content-between w-100">
                <a href="/Admin/Company/Upsert?id=${data}" class="btn btn-sm btn-primary w-100 me-1">Edit</a>
                <a href="/Admin/Company/Delete?id=${data}" class="btn btn-sm btn-danger w-100 me-1">Delete</a>
            </div>
        </div>
    `;
                },
            }
        ]
    });
}
