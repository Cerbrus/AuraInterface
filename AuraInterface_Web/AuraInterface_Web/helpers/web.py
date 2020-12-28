import json

from flask import Response

def ok(content = None):
    '''Return an OK (200) response'''
    return response(200, True, data = content)

def error(message = None):
    '''Return an ERROR (500) response'''
    return response(500, False, message = message)

def response(status, success, **kwargs):
    kwargs['success'] = success
    return Response(
        response = json.dumps({k: v for k, v in kwargs.items() if v is not None}),
        status   = status,
        mimetype = 'application/json')
