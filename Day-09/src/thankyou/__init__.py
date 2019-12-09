import logging

import azure.functions as func
from github import Github
import os


def main(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')

    # get payload
    req_body = req.get_json()

    # check that event is a created issue
    action = req_body.get('action')
    if action != 'opened':
        return func.HttpResponse(
            "Not an opened issue",
            status_code=204
        )

    # thank user for creating issue
    github = Github(os.environ["GITHUB_PAT"])
    repo = github.get_repo(os.environ["GITHUB_REPO"])
    issue = repo.get_issue(number=req_body['issue']['number'])
    username = req_body['issue']['user']['login']
    issue.create_comment('Thank you @' + username + '  you for submitting this issue. ' +
                         'We will take this on board and get back to you shortly!')

    return func.HttpResponse(
        "Thank you",
        status_code=200
    )
