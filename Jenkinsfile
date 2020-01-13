properties([
	gitLabConnection('gitlab'),
	parameters([string(defaultValue: 'refs/heads/master', description: 'the branch to build', name: 'branch', trim: true)])
])

node('windows2019') {
	try {
		gitlabCommitStatus("checkout") {
			stage('checkout'){
				checkout([$class: 'GitSCM', branches: [[name: params.branch]], doGenerateSubmoduleConfigurations: false, extensions: [[$class: 'LocalBranch', localBranch: "**"]], submoduleCfg: [], userRemoteConfigs: [[credentialsId: '361885ba-9425-4230-950e-0af201d90547', url: 'https://git.auckland.dynabic.com/assembly-cloud/assembly-net-sdk.git']]])
			}
		}
		gitlabCommitStatus("build") {
			stage('build') {
				bat 'docker build scripts -f scripts\\build.Dockerfile -t netsdkbuild'
				bat 'docker run -v %CD%:C:\\Build\\ netsdkbuild c:\\build\\scripts\\build.bat' 
			}
		}
		gitlabCommitStatus("prepare test env") {
			stage('prepare test env') {		
				bat 'mkdir testResults'
				bat 'mkdir Settings'
				withCredentials([usernamePassword(credentialsId: '6839cbe8-39fa-40c0-86ce-90706f0bae5d', passwordVariable: 'AppKey', usernameVariable: 'AppSid')]) {
					bat "echo {\"AppSid\":\"%AppSid%\",\"AppKey\":\"%AppKey%\" } > Settings\\servercreds.json"
				}
			}
		}
		gitlabCommitStatus("net tests") {
			stage('net tests') {	
				try {
					bat 'docker run -v %CD%:C:\\Build\\ netsdkbuild c:\\build\\scripts\\net-test.bat Tests'
				} finally {
					junit '**\\testResults\\Tests-results-net452.xml'
				}
			}
		}
	} finally {
		bat 'docker system prune -f'
		deleteDir()
	}
}