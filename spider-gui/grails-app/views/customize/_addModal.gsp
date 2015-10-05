<form class="form-horizontal" id="addModalForm">
<!-- Modal -->
<div class="modal fade" id="addRepoModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
        <h4 class="modal-title" id="myModalLabel">Add repository</h4>
      </div>
      <div class="modal-body">
      
       	<fieldset>
       	<div class="form-group">
			<label class="control-label"for="addName">
				Name </label>
			<div class="controls">
				<g:textField id="addName" name="addName" class="form-control"
					type="text" value="" required="required"/>
			</div>
		</div>
       	
       	<div class="form-group">
			<label class="control-label" for="name">
				Url </label>
			<div class="controls">
				<g:textField id="addUrl" name="addUrl" class="form-control"
					type="text" value="" required="required" url="url"/>
			</div>
		</div>
       	
       	</fieldset>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
        <button type="button" class="btn btn-primary" onClick="addRepoEntry()">Add</button>
      </div>
    </div>
  </div>
  </div>
  </form>