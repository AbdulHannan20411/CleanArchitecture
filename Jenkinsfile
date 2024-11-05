pipeline {
    agent any

    environment {
        BUILD_DIR = "D:\\My Projects\\Build\\ToDo_Build"  // Directory for deployment
    }

    stages {
        stage('Checkout') {
            steps {
                // Clone the repository
                git branch: 'master', url: 'https://github.com/AbdulHannan20411/CleanArchitecture.git'
            }
        }

        stage('Restore Dependencies') {
            steps {
                // Restore dependencies using .NET CLI
                bat 'dotnet restore ToDoApplication.sln' // Run this from the workspace root where the solution is located
            }
        }

        stage('Build') {
            steps {
                // Build the project in Release mode
                bat 'dotnet build --configuration Release ToDoApplication.sln' // Run this from the workspace root
            }
        }

        stage('Test') {
            steps {
                // Run tests
                bat 'dotnet test ToDoApplication.sln' // Run this from the workspace root
            }
        }

        stage('Publish') {
            steps {
                // Publish the build to a folder for deployment
                bat "dotnet publish -c Release -o ${env.BUILD_DIR} ToDoApplication.sln" // Run this from the workspace root
            }
        }

        stage('Deploy') {
            steps {
                // Deploy the published files (if needed)
                echo "Deploying files to ${env.BUILD_DIR}..."
                // Add your deployment logic here
            }
        }
    }

    post {
        always {
            echo 'Cleaning up workspace'
            deleteDir()  // Clean up the workspace after build
        }
        success {
            echo 'Build and deployment successful!'
        }
        failure {
            echo 'Build or deployment failed.'
        }
    }
}
