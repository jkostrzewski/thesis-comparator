
<form class="form-horizontal" enctype="multipart/form-data">
	<fieldset>
		<div class="form-group">
			<label class="control-label" for="dropzone"> Upload file form WEB </label>
			<div class="controls input-group">
				<g:textField id="file-from-url" name="file-from-url" class="form-control"
						type="text" 
						placeholder="http://www.exampleURL.com/"  url="url"/>
				 <span class="input-group-btn">
                  <button type="button" class="btn btn-primary" onClick="uploadManualy()"><i class="fa fa-upload"></i> Upload</button>
              </span>
			</div>
		</div>
		
		<div class="form-group">
			<label class="control-label" for="dropzone"> Upload files from local disc </label>
			<div class="controls">
				<div class="dropzone dropzone-previews" id="dropzone" style=""></div>
			</div>
		</div>
	</fieldset>
</form>

