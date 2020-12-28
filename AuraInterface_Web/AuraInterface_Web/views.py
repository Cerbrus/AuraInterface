'''
Routes and views for the flask application.
'''

from AuraInterface_Web import app
from datetime import datetime
from flask import render_template

from AuraInterface_Web.helpers import io, colors

@app.route('/')
@app.route('/home')
def home():
    '''Renders the home page.'''

    return render_template(
        'index.html',
        title         = 'Home',
        year          = datetime.now().year,
        current_color = io.get_current_color(),
        blocks        = 6
    )

@app.context_processor
def utility_processor():
    return dict(get_color = colors.get_color)