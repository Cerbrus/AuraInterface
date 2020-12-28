var __spreadArrays = (this && this.__spreadArrays) || function () {
    for (var s = 0, i = 0, il = arguments.length; i < il; i++) s += arguments[i].length;
    for (var r = Array(s), k = 0, i = 0; i < il; i++)
        for (var a = arguments[i], j = 0, jl = a.length; j < jl; j++, k++)
            r[k] = a[j];
    return r;
};
var AuraInterface;
(function (AuraInterface) {
    var Web;
    (function (Web) {
        var App = /** @class */ (function () {
            function App() {
                this.initializePicker();
                this.initializeTiles();
            }
            /**
             * Initialize the color picker
             */
            App.prototype.initializePicker = function () {
                var _this = this;
                var $container = $('.color-picker');
                if (!$container.length) {
                    return;
                }
                this.$picker_container = $container;
                var current_color = $container.data('color');
                this.picker = $container
                    .ColorPicker({
                    flat: true,
                    color: current_color,
                    onChange: function (hsb, hex, rgb) {
                        _this.$picker_container.attr('data-color', "#" + hex);
                    },
                    onSubmit: function (hsb, hex, rgb) {
                        _this.setColor("#" + hex);
                    }
                });
            };
            /**
             * Initialize the click handler for tiles.
             */
            App.prototype.initializeTiles = function () {
                $(document).on('click', '.tiles-tile', this.onTileClick.bind(this));
            };
            /**
             * Handle the user's click on a tile.
             * @param {JQueryMouseEventObject} evt
             */
            App.prototype.onTileClick = function (evt) {
                var color = evt.target.title;
                this.setColor(color);
            };
            /**
             * Set the color
             * @param {string} color
             */
            App.prototype.setColor = function (color) {
                var _this = this;
                $.post('/api/v1/color', { color: color })
                    .fail(function () {
                    var error = [];
                    for (var _i = 0; _i < arguments.length; _i++) {
                        error[_i] = arguments[_i];
                    }
                    console.warn.apply(console, __spreadArrays(['Something went wrong trying to set the color'], error));
                })
                    .done(function (response) {
                    _this.picker.ColorPickerSetColor(color);
                    _this.$picker_container.attr('data-color', color);
                });
            };
            return App;
        }());
        Web.App = App;
    })(Web = AuraInterface.Web || (AuraInterface.Web = {}));
})(AuraInterface || (AuraInterface = {}));
(function () {
    var app = new AuraInterface.Web.App();
}());
//# sourceMappingURL=app.js.map