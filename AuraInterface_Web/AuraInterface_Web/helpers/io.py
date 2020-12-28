import subprocess
import json

def get_current_color():    
    result = subprocess.run(['command_get_colors.bat'], capture_output = True)
    colors = json.loads(result.stdout)
    return colors[0]


def set_current_color(color):    
    result = subprocess.run(['command_set_color.bat', color], capture_output = True)