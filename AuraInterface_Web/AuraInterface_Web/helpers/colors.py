def get_color(parts, r, g, b):    
    def x(number, parts):
        factor = int(255 / parts)
        return max(0, number * factor)
    
    return '#%02x%02x%02x' % (x(r, parts), x(g, parts), x(b, parts))