
<div class="container">
<div class="col-md-12">
	<div class="page-header form-group">
		<h1>
			Customize <small>define your own repositories.</small>
		</h1>

	</div>
</div>

	
		<div class="col-md-12">
			<div class="row">
				<form class="form-horizontal" id="customizeForm">
					<div class="tabbable tabs-left">
						<div class="col-md-2">
							<button type="button" id="add-repo-button"
								class="btn btn-info repo-btn" onClick="showAddModal()" style="margin-bottom:10px">
								<i class="fa fa-plus"></i> Add
							</button>
							<ul class="nav nav-pills nav-stacked" id="repo-nav-list">
								<g:each in="${repositories}" var="repo" status="i">
									<g:render template="navEntry" model="[name: repo.name, id:i]" />
								</g:each>
							</ul>

						</div>
						<div class="col-md-10">
							<div class="container tab-content" id="repo-content-list">
								<g:each in="${repositories}" var="repo" status="i">
									<g:render template="contentEntry"
										model="[name:repo.name, url:repo.url, id:i, definedRepository:true]" />
								</g:each>

							</div>
						</div>
					</div>
					<input type="hidden" id="customizeCount" name="customizeCount"
				value="${repositories.size()}" />			
				</form>
				<g:render template="addModal" />
			</div>
		</div>
		<div class="col-md-10 col-md-offset-2" style="margin-top:15px">
		<div class="col-md-12">
					<div class="col-md-2 col-md-offset-8">
						<button type="button" id="back-button"
							class="btn btn-default repo-btn"
							onClick="loadIndexPage()">
							<i class="fa fa-arrow-left"></i> Back
						</button>
					</div>
					<div class="col-md-2">
						<button type="button" id="run-button"
							class="btn btn-success repo-btn"
							onClick="loadResultPage(true, true)">
							<i class="fa fa-play"></i> Run
						</button>
					</div>
					</div>
</div>
	</div>

