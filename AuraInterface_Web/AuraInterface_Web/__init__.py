"""
The flask application package.
"""

from flask import Flask, render_template
app = Flask(__name__)

import AuraInterface_Web.views
import AuraInterface_Web.api