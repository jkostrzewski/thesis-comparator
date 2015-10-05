<!DOCTYPE html>
<!--[if lt IE 7 ]> <html lang="en" class="no-js ie6"> <![endif]-->
<!--[if IE 7 ]>    <html lang="en" class="no-js ie7"> <![endif]-->
<!--[if IE 8 ]>    <html lang="en" class="no-js ie8"> <![endif]-->
<!--[if IE 9 ]>    <html lang="en" class="no-js ie9"> <![endif]-->
<!--[if (gt IE 9)|!(IE)]><!--> <html lang="en" class="no-js"><!--<![endif]-->
	<head>
		<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
		<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
		<title>Web Spider</title>
		<meta name="viewport" content="width=device-width, initial-scale=1.0">
		

		<link rel="stylesheet" href="${resource(dir: 'css', file: 'bootstrap.css')}" type="text/css">
		<link rel="stylesheet" href="${resource(dir: 'css', file: 'style.css')}" type="text/css">
		<link rel="stylesheet" href="${resource(dir: 'css', file: 'style-custom.css')}" type="text/css">
		<link rel="stylesheet" href="${resource(dir: 'css', file: 'pills.css')}" type="text/css">
		<link rel="stylesheet" href="${resource(dir: 'css', file: 'chosen.css')}" type="text/css">
		<link rel="stylesheet" href="${resource(dir: 'css', file: 'dropzone.css')}" type="text/css">
		<link rel="stylesheet" href="${resource(dir: 'css', file: 'datatables.css')}" type="text/css">
			<link rel="stylesheet" href="${resource(dir: 'css', file: 'jquery.pnotify.default.css')}" type="text/css">
		<link rel="stylesheet" href="${resource(dir: 'css', file: 'jquery.pnotify.default.icons.css')}" type="text/css">
		
		
		<link href="//netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.css" rel="stylesheet">
		<g:javascript src="jquery.js" />
		<g:javascript src="bootstrap.js" />
		<g:javascript src="chosen.jquery.js" />
		<g:javascript src="web-flow.js" />
		<g:javascript src="dropzone.js"/>
		<g:javascript src="jquery.validate.js"/>
		<g:javascript src="jquery.dataTables.js"/>
		<g:javascript src="jquery.pnotify.js"/>
		
		
		
		
		<r:layoutResources />
	</head>
	<body>
		<header>
		<g:render template="../layouts/header"/>
		</header>
		<div id="main-content">
		</div>
		<g:hiddenField name="autostart" id="autostart" value="${autostart}"/>
		<g:javascript library="application"/>
		<r:layoutResources />
	</body>
	<script type="text/javascript">
	$(document).ready(function() {
		Application = {
				Index : {
					Url: "${g.createLink(controller:'index', action:'show')}"
				},
				Customize :{
					Url: "${g.createLink(controller:'customize', action:'show')}",
					addRepository: "${g.createLink(controller:'customize', action:'addRepository')}",
				},
				Result : {
					Url: "${g.createLink(controller:'result', action:'show')}",
					DataGrid: "${g.createLink(controller:'Db', action:'getData')}",
					UploadLocalFileUrl: "${g.createLink(controller:'Db', action:'uploadLocalFile')}",
					uploadManualyUrl: "${g.createLink(controller:'Db', action:'uploadManualy')}",			
					DeleteFile: "${g.createLink(controller:'Db', action:'deleteFile')}",
					GetOriginalFile: "${g.createLink(controller:'Db', action:'getOriginalFile')}",
					GetRawFile: "${g.createLink(controller:'Db', action:'getRawFile')}",
					DropDB: "${g.createLink(controller:'Db', action:'dropDB')}"
				},
				Search:{
					defaultSearch: "${g.createLink(controller:'search', action:'defaultSearch')}",
					customizeSearch: "${g.createLink(controller:'search', action:'customizeSearch')}",
					terminateSearch: "${g.createLink(controller:'search', action:'terminateSearch')}"
					}
		}
		if ($('#autostart').val()==1){
				loadResultPage(false, true)
			}
		else{
				loadIndexPage();
		}
		setValidatorDefaults();
	});

	var dataGrid 
	$.fn.dataTableExt.oApi.fnReloadAjax = function ( oSettings, sNewSource, fnCallback, bStandingRedraw )
	{
	    // DataTables 1.10 compatibility - if 1.10 then versionCheck exists.
	    // 1.10s API has ajax reloading built in, so we use those abilities
	    // directly.
	    if ( $.fn.dataTable.versionCheck ) {
	        var api = new $.fn.dataTable.Api( oSettings );
	 
	        if ( sNewSource ) {
	            api.ajax.url( sNewSource ).load( fnCallback, !bStandingRedraw );
	        }
	        else {
	            api.ajax.reload( fnCallback, !bStandingRedraw );
	        }
	        return;
	    }
	 
	    if ( sNewSource !== undefined && sNewSource !== null ) {
	        oSettings.sAjaxSource = sNewSource;
	    }
	 
	    // Server-side processing should just call fnDraw
	    if ( oSettings.oFeatures.bServerSide ) {
	        this.fnDraw();
	        return;
	    }
	 
	    this.oApi._fnProcessingDisplay( oSettings, true );
	    var that = this;
	    var iStart = oSettings._iDisplayStart;
	    var aData = [];
	 
	    this.oApi._fnServerParams( oSettings, aData );

	    oSettings.fnServerData.call( oSettings.oInstance, oSettings.sAjaxSource, aData, function(json) {
	        /* Clear the old information from the table */
	        that.oApi._fnClearTable( oSettings );
	 
	        /* Got the data - add it to the table */
	        var aData =  (oSettings.sAjaxDataProp !== "") ?
	            that.oApi._fnGetObjectDataFn( oSettings.sAjaxDataProp )( json ) : json;
	 
	        for ( var i=0 ; i<aData.length ; i++ )
	        {
	            that.oApi._fnAddData( oSettings, aData[i] );
	        }
	         
	        oSettings.aiDisplay = oSettings.aiDisplayMaster.slice();
	 
	        that.fnDraw();
	 
	        if ( bStandingRedraw === true )
	        {
	            oSettings._iDisplayStart = iStart;
	            that.oApi._fnCalculateEnd( oSettings );
	            that.fnDraw( false );
	        }
	 
	        that.oApi._fnProcessingDisplay( oSettings, false );
	 
	        /* Callback user function - for event handlers etc */
	        if ( typeof fnCallback == 'function' && fnCallback !== null )
	        {
	            fnCallback( oSettings );
	        }

	    }, oSettings );
	};

	

	/* Set the defaults for DataTables initialisation */
	$.extend(true, $.fn.dataTable.defaults, {
	  "sDom": "<'row'<'col-xs-5 col-sm-6'l><'col-xs-7 col-sm-6 text-right'f>r>t<'row'<'col-xs-3 col-sm-4 col-md-5'i><'col-xs-9 col-sm-8 col-md-7 text-right'p>>",
	  "sPaginationType": "bootstrap",
	  "oLanguage": {
	    "sLengthMenu": "_MENU_ per page "
	  },
	  "fnInitComplete": function (oSettings, json) {
	    var currentId = $(this).attr('id');
	    console.log(currentId);
	    if (currentId) {
	 

		      var thisLength = $('#' + currentId + '_length');
		      var thisLengthLabel = $('#' + currentId + '_length label');
		      var thisLengthSelect = $('#' + currentId + '_length label select');
		 
		      var thisFilter = $('#' + currentId + '_filter');
		      var thisFilterLabel = $('#' + currentId + '_filter label');
		      var thisFilterInput = $('#' + currentId + '_filter label input');
		 
		      // Re-arrange the records selection for a form-horizontal layout
		      thisLength.addClass('form-group');
		      thisLengthLabel.addClass('control-label col-md-3').attr('for', currentId + '_length_select').css('text-align', 'left');
		      thisLengthSelect.addClass('form-control input-sm').attr('id', currentId + '_length_select');
		      thisLengthSelect.prependTo(thisLength).wrap('<div class="col-md-3" />');
		      // Re-arrange the search input for a form-horizontal layout
		      thisFilter.addClass('form-group');
		      thisFilterLabel.addClass('control-label col-md-3').attr('for', currentId + '_filter_input').css('text-align', 'right');;
		      thisFilterInput.addClass('form-control input-sm').attr('id', currentId + '_filter_input');
		      thisFilterInput.appendTo(thisFilter).wrap("<div class='col-md-3'>").css('text-align', 'right');
		      $(thisFilter).append("<div class='col-md-3'><button type='button' class='btn btn-default btn-sm' onclick='terminateSearch()''><i class='fa fa-times-circle'></i> Terminate</button></div>")
		      $(thisFilter).append("<div class='col-md-3'><button type='button' class='btn btn-danger btn-sm' onClick='dropDB()'><i class='fa fa-eraser'></i> Clear database</button></div>")
		      
	    }
	  }
	});
	 
	$.extend($.fn.dataTableExt.oStdClasses, {
	  "sWrapper": "dataTables_wrapper form-horizontal"
	});
	 
	/* API method to get paging information */
	$.fn.dataTableExt.oApi.fnPagingInfo = function (oSettings) {
	  return {
	    "iStart": oSettings._iDisplayStart,
	    "iEnd": oSettings.fnDisplayEnd(),
	    "iLength": oSettings._iDisplayLength,
	    "iTotal": oSettings.fnRecordsTotal(),
	    "iFilteredTotal": oSettings.fnRecordsDisplay(),
	    "iPage": oSettings._iDisplayLength === -1 ? 0 : Math.ceil(oSettings._iDisplayStart / oSettings._iDisplayLength),
	    "iTotalPages": oSettings._iDisplayLength === -1 ? 0 : Math.ceil(oSettings.fnRecordsDisplay() / oSettings._iDisplayLength)
	  };
	};
	 
	 
	/* Bootstrap style pagination control */
	$.extend($.fn.dataTableExt.oPagination, {
	  "bootstrap": {
	    "fnInit": function (oSettings, nPaging, fnDraw) {
	      var oLang = oSettings.oLanguage.oPaginate;
	      var fnClickHandler = function (e) {
	        e.preventDefault();
	        if (oSettings.oApi._fnPageChange(oSettings, e.data.action)) {
	          fnDraw(oSettings);
	        }
	      };
	 
	      $(nPaging).append(
	       '<ul class="pagination">' +
	        '<li class="first disabled"><a href="#" title="' + oLang.sFirst + '"><span class="glyphicon glyphicon-fast-backward"></span></a></li>' +
	        '<li class="prev disabled"><a href="#" title="' + oLang.sPrevious + '"><span class="glyphicon glyphicon-chevron-left"></span></a></li>' +
	        '<li class="next disabled"><a href="#" title="' + oLang.sNext + '"><span class="glyphicon glyphicon-chevron-right"></span></a></li>' +
	        '<li class="last disabled"><a href="#" title="' + oLang.sLast + '"><span class="glyphicon glyphicon-fast-forward"></span></a></li>' +
	       '</ul>'
	      );
	      var els = $('a', nPaging);
	      $(els[0]).bind('click.DT', { action: "first" }, fnClickHandler);
	      $(els[1]).bind('click.DT', { action: "previous" }, fnClickHandler);
	      $(els[2]).bind('click.DT', { action: "next" }, fnClickHandler);
	      $(els[3]).bind('click.DT', { action: "last" }, fnClickHandler);
	    },
	 
	    "fnUpdate": function (oSettings, fnDraw) {
	      var iListLength = 5;
	      var oPaging = oSettings.oInstance.fnPagingInfo();
	      var an = oSettings.aanFeatures.p;
	      var i, j, sClass, iStart, iEnd, iHalf = Math.floor(iListLength / 2);
	 
	        if (oPaging.iTotalPages < iListLength) { iStart = 1; iEnd = oPaging.iTotalPages; } else if (oPaging.iPage <= iHalf) { iStart = 1; iEnd = iListLength; } else if (oPaging.iPage >= oPaging.iTotalPages - iHalf) { iStart = oPaging.iTotalPages - iListLength + 1; iEnd = oPaging.iTotalPages; } else { iStart = oPaging.iPage - iHalf + 1; iEnd = iStart + iListLength - 1; }
	 
	        for (i = 0, iLen = an.length ; i < iLen ; i++) {
	          // Remove the middle elements
	          $('li:gt(1)', an[i]).filter(':not(.next,.last)').remove();
	 
	          // Add the new list items and their event handlers
	          for (j = iStart; j <= iEnd; j++) { sClass = j == oPaging.iPage + 1 ? 'class="active"' : ""; $("<li " + sClass + '><a href="#">' + j + "</a></li>").insertBefore($(".next,.last", an[i])[0]).bind("click", function (a) { a.preventDefault(); oSettings._iDisplayStart = (parseInt($("a", this).text(), 10) - 1) * oPaging.iLength; fnDraw(oSettings) }) }
	 
	          // Add / remove disabled classes from the static elements
	          if (oPaging.iPage === 0) $(".first,.prev", an[i]).addClass("disabled"); else $(".first,.prev", an[i]).removeClass("disabled")
	 
	          if (oPaging.iPage === oPaging.iTotalPages - 1 || oPaging.iTotalPages === 0) $(".next,.last", an[i]).addClass("disabled"); else $(".next,.last", an[i]).removeClass("disabled")
	        }
	    }
	  }
	});
	 
	
	</script>
</html>
