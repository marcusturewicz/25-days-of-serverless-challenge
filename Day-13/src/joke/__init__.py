import logging
import os
import azure.functions as func
from textgenrnn import textgenrnn

def main(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')

    textgen = textgenrnn()
    textgen.train_from_file('/workspaces/25-days-of-serverless-challenge/Day-13/src/joke/jokes.txt', num_epochs=1)
    joke = textgen.generate()

    return func.HttpResponse(
        joke,
        status_code=200
    )
