"""
API endpoints for the flask application.
"""
from flask import request

from AuraInterface_Web import app
from AuraInterface_Web.helpers import io, colors, web

@app.route('/api/v1/color', methods=['GET'])
def get_color():
    '''Get the current color'''
    color = io.get_current_color()
    return web.ok(color)

@app.route('/api/v1/color', methods=['POST'])
def set_color():
    '''Set the current color'''
    if 'color' in request.values:
        color = request.values['color']
    else:
        return web.error('No color provided.')

    if len(color) is 0:
        return web.error('Invalid color provided.')

    io.set_current_color(color)

    return web.ok()