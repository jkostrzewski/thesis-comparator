function loadIndexPage() {
	var data = 'asd'
	jQuery.post(Application.Index.Url, data, function(html) {
		fadeReload('main-content', html)
	});
}

function loadCustomizePage() {
	var data = 'asd'
	jQuery.post(Application.Customize.Url, data, function(html) {
		fadeReload('main-content', html)
		$('#main-content').promise().done(function() {
			applyCustomizeTab()
			$('#customizeForm').validate({
				ignore : ""
			})
			$('#addModalForm').validate()
		})
	});
}

function fadeReload(target, html) {
	$('#' + target).fadeOut('fast', function() {
		$('#' + target).html(html);
		$('#' + target).fadeIn('fast');

	});
}

function applyCustomizeTab() {
	$(".chosen-select").chosen();
}

function removeRepo(id) {
	$('#repo-' + id).fadeOut(function() {
		$('#repo-' + id).remove();
		$('#repo-content-list > :first-child').addClass('active')

	})
	$('#repo-nav-' + id).fadeOut(function() {
		$('#repo-nav-' + id).remove();
		$('#repo-nav-list > :first-child').addClass('active')
	})
	$.pnotify({
		title : 'Repository removed!',
		text : 'You have successfully removed repository.',
		type : 'success'
	});
}

function showAddModal() {
	$('#addRepoModal').modal()
}

function addRepoEntry() {
	if (!$('#addModalForm').valid())
		return;

	var data = {
		name : $('#addName').val(),
		url : $('#addUrl').val(),
		count : $('#customizeCount').val()
	}
	jQuery.post(Application.Customize.addRepository, data, function(response) {
		$('#repo-nav-list').append(response.navEntry)
		$('#repo-content-list').append(response.contentEntry)
		highlightLastRepo();
		applyCustomizeTab();
		$('#customizeCount').val(response.count)
		$.pnotify({
			title : 'Repository created!',
			text : 'You have successfully created repository.',
			type : 'success'
		});
	});
	$('#addRepoModal').find('input:text').val('')
	$('#addRepoModal').modal('hide')
}

function highlightLastRepo() {
	$('#repo-nav-list').children().removeClass('active')
	$('#repo-content-list').children().removeClass('active')
	$('#repo-nav-list > :last-child').addClass('active')
	$('#repo-content-list > :last-child').addClass('active')
}

function loadResultPage(customize, dataGridRefresh) {
	var data = null
	var searchUrl
	if (customize) {
		if (!$('#customizeForm').valid()) {
			$.pnotify({
				title : 'Form is invalid!',
				text : 'Please verify your repositories settings.',
				type : 'error'
			});
			return;
		}
		searchUrl = Application.Search.customizeSearch
		data = $('#customizeForm').serialize()
	} else {
		searchUrl = Application.Search.defaultSearch
	}

	if (!dataGridRefresh) {
		searchUrl = Application.Result.Url
	}
	jQuery
			.post(
					searchUrl,
					data,
					function(html) {
						fadeReload('main-content', html)
						$('#main-content')
								.promise()
								.done(
										function() {
											applyResultPage()
											applyDataGrid()
											if (dataGridRefresh) {
												refreshDataGrid()
												$
														.pnotify({
															title : 'Robot is working <img src="images/preloader.gif" width="137px" height="12px"/>',
															type : 'info',
															hide : false
														});
											}
										})

					});
}

function applyResultPage() {
	applyDropzone()
}

function applyDropzone() {
	$('#dropzone').dropzone(
			{
				url : Application.Result.UploadLocalFileUrl,
				previewsContainer : '.dropzone-previews',
				uploadMultiple : true,
				paramName : 'file',
				multipart : true,
				enqueueForUpload: true,
				init : function() {
					this.on("success", function(file, responseText) {
						dataGrid.fnReloadAjax(Application.Result.DataGrid,
								null, true);
						var _this = this
						setTimeout(function() {
							dataGrid.fnReloadAjax(Application.Result.DataGrid,
									function() {
										_this.removeFile(file)
									}, true);

						}, 1000)

					});
				}
			});
}

function dropDB() {
	jQuery.post(Application.Result.DropDB, null, function(response) {
		$.pnotify({
			title : 'Database cleared!',
			text : 'You have successfully cleared database.',
			type : 'success'
		});
		dataGrid.fnReloadAjax(Application.Result.DataGrid, null, true);
	});
}

function uploadManualy() {
	var data = {
		url : $('#file-from-url').val()
	}
	if (!$('#file-from-url').valid() || $('#file-from-url').val()==''){
		return
	}
	jQuery.post(Application.Result.uploadManualyUrl, data, function(response) {
		$('#file-from-url').val('')
		$.pnotify({
			title : 'Inserted!',
			text : 'You have correctly inserted file.',
			type : 'success'
		});
		dataGrid.fnReloadAjax(Application.Result.DataGrid, null, true);
	});
}

function setValidatorDefaults() {
	$.validator.setDefaults({
		errorElement : "span",
		errorClass : "help-block",
		highlight : function(element, errorClass, validClass) {
			$(element).closest('.form-group').addClass('has-error');
		},
		unhighlight : function(element, errorClass, validClass) {
			$(element).closest('.form-group').removeClass('has-error');
		},
		errorPlacement : function(error, element) {
			if (element.parent('.input-group').length
					|| element.prop('type') === 'checkbox'
					|| element.prop('type') === 'radio') {
				error.insertAfter(element.parent());
			} else {
				error.insertAfter(element);
			}
		}
	});

}

function applyDataGrid() {
	dataGrid = $('#dataGrid')
			.dataTable(
					{
						"sDom" : "<'row'<'col-xs-5 col-sm-6'l><'col-xs-7 col-sm-6 text-right'f>r>t<'row'<'col-xs-3 col-sm-4 col-md-5'i><'col-xs-9 col-sm-8 col-md-7 text-right'p>>",
						"bServerSide" : false,
						"sAjaxSource" : Application.Result.DataGrid,
						"bAutoWidth" : false,
						"aoColumns" : [
								{
									"mData" : "filename",
									sDefaultContent : "n/a",
									"sClass" : "filename"
								},
								{
									"mData" : "length",
									sDefaultContent : "n/a",
									"sClass" : "length",
									"sWidth" : "90px"
								},
								{
									"mData" : "uploadDate",
									sDefaultContent : "n/a",
									"sClass" : "uploadDate",
									"sWidth" : "185px"
								},
								{
									"mData" : "orginalUrl",
									sDefaultContent : "n/a",
									"sClass" : "orginalUrl"
								},
								{
									"mData" : "keywords",
									sDefaultContent : "n/a",
									"sClass" : "keywords",
									"sWidth" : "180px"
								},
								{
									"bAutoWidth" : false,
									"sWidth" : "140px",
									"bSortable" : false,
									"mDataProp" : null,
									"sClass" : "control center",
									"sDefaultContent" : '<button type="button" class="btn btn-info originalIco" onClick="getOriginalFile(this)">'
											+ '<i class="fa fa-file-o"></i></button>'
											+

											'<button type="button" class="btn btn-default rawIco" onClick="getRawFile(this)">'
											+ '<i class="fa fa-file-text-o"></i></button>'
											+

											'<button type="button" class="btn btn-danger deleteIco" onClick="deleteTableFile(this)">'
											+ '<i class="fa fa-trash-o"></i></button>'
								}, {
									"mData" : "fileId",
									"bVisible" : false
								} ],
						"aLengthMenu" : [ [ 5, 25, 50, -1 ],
								[ 5, 25, 50, "All" ] ],
						"iDisplayLength" : 5,
					});
	dataGrid.fnSort([ [ 2, 'desc' ] ]);

}

function deleteTableFile(x) {

	var tr = $(x).closest("tr").get(0)
	var position = dataGrid.fnGetPosition(tr);
	aData = dataGrid.fnGetData(position).fileId
	data = {
		aData:aData
	}
	jQuery.post(Application.Result.DeleteFile, data, function(response) {
		$.pnotify({
			title : 'Deleted!',
			text : 'You have successfully deleted file.',
			type : 'success'
		});
		dataGrid.fnReloadAjax(Application.Result.DataGrid, null, true);
	});
}

function getOriginalFile(x) {
	var tr = $(x).closest("tr").get(0)
	var position = dataGrid.fnGetPosition(tr);
	aData = dataGrid.fnGetData(position).fileId
	
	window.open(Application.Result.GetOriginalFile + "?id=" + aData, 'Original File')

}

function getRawFile(x) {
	var tr = $(x).closest("tr").get(0)
	var position = dataGrid.fnGetPosition(tr);
	aData = dataGrid.fnGetData(position).fileId
	
	window.open(Application.Result.GetRawFile + "?id=" + aData, 'Original File')
}

var gridIntervalId
var dataGridDataReceived = true
function refreshDataGrid() {
	gridIntervalId = setInterval(function() {
		if (dataGridDataReceived == true) {
			dataGridDataReceived = false
			dataGrid.fnReloadAjax(Application.Result.DataGrid,
					getGridDataCheck, true);
		}
	}, 5000); // milis
}

function getGridDataCheck(settings) {
	response = jQuery.parseJSON(settings.jqXHR.responseText);
	if (response.searchEnded == true) {
		clearInterval(gridIntervalId)
		$.pnotify_remove_all();
		$.pnotify({
			title : 'Searching finished!',
			text : 'You can manage your search results now.',
			type : 'success'
		});
	}
	dataGridDataReceived = true
}


function terminateSearch() {
	jQuery.post(Application.Search.terminateSearch, null, function() {
	});
}