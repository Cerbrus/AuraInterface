namespace AuraInterface.Web {
    export class App {
        private $picker_container: JQuery<HTMLElement>;
        private picker: any;

        constructor() {
            this.initializePicker();
            this.initializeTiles();
        }

        /**
         * Initialize the color picker
         */
        private initializePicker(): void {
            const $container = $('.color-picker');

            if (!$container.length) {
                return;
            }

            this.$picker_container = $container;

            const current_color = $container.data('color');

            this.picker = ($container as any)
                .ColorPicker({
                    flat: true,
                    color: current_color,
                    onChange: (hsb, hex, rgb) => {
                        this.$picker_container.attr('data-color', `#${hex}`);
                    },
                    onSubmit: (hsb, hex, rgb) => {
                        this.setColor(`#${hex}`);
                    }
                });
        }

        /**
         * Initialize the click handler for tiles.
         */
        private initializeTiles(): void {
            $(document).on('click', '.tiles-tile', this.onTileClick.bind(this));
        }

        /**
         * Handle the user's click on a tile.
         * @param {JQueryMouseEventObject} evt
         */
        private onTileClick(evt: JQueryMouseEventObject): void {
            const color = (evt.target as HTMLSpanElement).title;
            this.setColor(color);
        }

        /**
         * Set the color
         * @param {string} color
         */
        private setColor(color: string) {
            $.post('/api/v1/color', { color })
                .fail((...error) => {
                    console.warn('Something went wrong trying to set the color', ...error);
                })
                .done(response => {
                    this.picker.ColorPickerSetColor(color);
                    this.$picker_container.attr('data-color', color);
                });
        }
    }

}

(function () {
    const app = new AuraInterface.Web.App();
}());