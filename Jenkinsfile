#!groovy

def FAILED_STAGE

pipeline {
  agent none

  environment {
    REPO_NAME = "braintree-dotnet"
    SLACK_CHANNEL = "#auto-team-sdk-builds"
  }

  options {
    buildDiscarder(logRotator(numToKeepStr: '50'))
    timestamps()
    timeout(time: 120, unit: 'MINUTES')
  }

  stages {
    stage("Audit") {
      parallel {
        stage("CodeQL") {
          agent {
            node {
              label ""
              customWorkspace "workspace/${REPO_NAME}"
            }
          }

          steps {
            codeQLv2(csharp: true)
          }

          post {
            failure {
              script {
                FAILED_STAGE = env.STAGE_NAME
              }
            }
          }
        }

        stage("SonarQube") {
          agent {
            node {
              label ""
              customWorkspace "workspace/${REPO_NAME}-sonar"
            }
          }

          steps {
            script {
              sh "docker build -t braintree-dotnet -f Dockerfile-core3 ."
              sh "docker run --rm -v \"\$(pwd):\$(pwd)\" -w \"\$(pwd)\" braintree-dotnet /bin/bash -l -c 'dotnet test test/Braintree.Tests/Braintree.Tests.csproj --framework netcoreapp3.1 /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=./coverage/'"
              executeSonarQubeScan()
            }
          }

          post {
            failure {
              script {
                FAILED_STAGE = env.STAGE_NAME
              }
            }
          }
        }
      }
    }

    stage("SDK Tests") {
      when {
        branch 'master'
      }

      parallel {
        stage("Dotnet Mono 6.8.0 Buster") {
          agent {
            node {
              label ""
              customWorkspace "workspace/${REPO_NAME}"
            }
          }

          steps {
            build job: 'dotnet_mono-6.8.0-buster_server_sdk_master', wait: true
          }

          post {
            failure {
              script {
                FAILED_STAGE = env.STAGE_NAME
              }
            }
          }
        }

        stage("Dotnet Core 3.1.4 Buster") {
          agent {
            node {
              label ""
              customWorkspace "workspace/${REPO_NAME}"
            }
          }

          steps {
            build job: 'dotnet_core-3.1.4-buster_server_sdk_master', wait: true
          }

          post {
            failure {
              script {
                FAILED_STAGE = env.STAGE_NAME
              }
            }
          }
        }
      }
    }
  }

  post {
    unsuccessful {
      slackSend color: "danger",
        channel: "${env.SLACK_CHANNEL}",
        message: "${env.JOB_NAME} - #${env.BUILD_NUMBER} Failure after ${currentBuild.durationString} at stage \"${FAILED_STAGE}\"(<${env.BUILD_URL}|Open>)"
    }
  }
}

