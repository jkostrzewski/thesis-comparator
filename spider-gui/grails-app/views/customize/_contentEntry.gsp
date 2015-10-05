<div class="tab-pane ${id==0?'active':''}" id="repo-${id}">
	<fieldset>
		<div class="row">
			<div class="col-md-10">
				<g:if test="${name}">
					<div class="form-group">
						<label class="control-label" for="name"> Name </label>
						<div class="controls">
							${name}
							<g:hiddenField name="name-${id}" value="${name}" />
						</div>
					</div>
				</g:if>
			</div>

			<div class="col-md-2">
				<div class="form-group">

					<div class="controls">
						<button type="button" class="btn btn-danger remove-repo-btn"
							onClick="removeRepo('${id}')">
							<i class="fa fa-trash-o"></i> Remove
						</button>
					</div>
				</div>

			</div>

		</div>	
			<div class="form-group">
				<label class="control-label" data-validation="name_label" for="name">
					Url </label>
				<div class="controls">
					<g:textField id="url-${id}" name="url-${id}" class="form-control"
						type="text" value="${url}"
						placeholder="http://www.examplerepository.com/" url="url" required="required"/>
				</div>
			</div>
		<g:if test="${definedRepository}">
			<div class="form-group">
				<label class="control-label" data-validation="name_label" for="name">
					Keywords </label>
				<div class="controls">
					<g:textField id="keywords-${id}" name="keywords-${id}"
						class="form-control" type="text" value="${keywords}"
						placeholder="list of keywords, comma-separated." required="required"/>
				</div>
			</div>
		</g:if>



		<div class="form-group">
			<label class="control-label" data-validation="name_label" for="name">
				Accepted types </label>
			<div class="controls">
				<g:select id="accepted-types-${id}" name="accepted-types-${id}"
					class="chosen-select form-control" multiple="true"
					from="${dictionary?.acceptedTypes?.items?.ext}" keys="${dictionary?.acceptedTypes?.items?.mime}" />
			</div>
		</div>

		<div class="row">
			<div class="col-md-6">
				<div class="form-group">
					<label class="control-label" for="threads"> No. threads </label>
					<div class="controls">
						<g:textField id="threads-${id}" name="threads-${id}"
							class="form-control" type="text" value="10" placeholder="e.g. 10" max="300" min="1" number="number" required="required"/>
					</div>
				</div>
			</div>
			<div class="col-md-6">
				<div class="form-group">
					<label class="control-label" for="depth"> Search depth </label>
					<div class="controls">
						<g:textField id="depth-${id}" name="depth-${id}"
							class="form-control" type="text" value="2" placeholder="e.g. 3" max="10" min="1" number="number" required="required"/>
					</div>
				</div>
			</div>
		</div>

		<div class="row">
			<div class="col-md-4">
				<div class="form-group">
					<label class="control-label" for="priority"> Languages </label>
					<div class="controls">
						<g:select id="languages-${id}" name="languages-${id}"
					class="chosen-select form-control" multiple="true"
					from="${dictionary?.languages?.items?.name}" keys="${dictionary?.languages?.items?.mime}" />
					</div>
				</div>
			</div>
			<div class="col-md-4">
				<div class="form-group">
					<label class="control-label" for="max-pages"> Max pages </label>
					<div class="controls">
						<g:textField id="max-pages-${id}" name="max-pages-${id}"
							class="form-control" type="text" value="" placeholder="Unlimited" number="number"/>
					</div>
				</div>
			</div>
			<div class="col-md-4">
				<div class="form-group">
					<label class="control-label" for="save-original"> Save
						original </label>
					<div class="controls">
						<g:select id="save-original-${id}" name="save-original-${id}"
							class="form-control" from="['Yes','No']" keys="[true, false]" />
					</div>
				</div>
			</div>
		</div>

	</fieldset>
</div>