(function (window, $) {
    'use strict';

    var d;
    function getYTScript() {
        if (!d) {
            d = $.Deferred();

            $.getScript('https://www.youtube.com/iframe_api');

            window.onYouTubeIframeAPIReady = function () {
                d.resolve(window.YT);
            };
        }

        return d;
    }

    var ytPlayer = function ($player) {
        var elements = {};
        var player;
        var isPlayerLoaded = false;

        function _cacheElements() {
            elements.$video = $player.find('.yt__video');
            elements.$screenshot = $player.find('.yt__screenshot');
            elements.$playButton = $player.find('.yt__play');
        }

        function _createPlayer() {
            player = new window.YT.Player(elements.$video.get(0), {
                videoId: elements.$video.data('yt-id'),
                width: '100%',
                height: '100%',
                playerVars: { modestbranding: 1, rel: 0 },
                events: {
                    onReady: _onPlayerReady,
                    onStateChange: _onPlayerStateChange
                }
            });
            elements.$video = null;
        }

        function _handleClick(e) {
            e.preventDefault();
            elements.$screenshot
                .animate({ opacity: 0 }, 600, function () {
                    $(this).prev().css({ 'z-index': 1 });
                    player.playVideo();
                });

            return false;
        }

        function _onPlayerReady() {
            isPlayerLoaded = true;
            elements.$playButton
                .on('click', _handleClick);
        }

        function _onPlayerStateChange(event) {
            if (event.data == 0) {
                showScreenshot();
            }
        }

        function _killPlayback() {
            $('.video-modal .close').on('click', function () {
                stopPlayer();
            });
        }

        function _stopPlayback() {
            $('.video-modal').on('hidden.bs.modal', function () {
                stopPlayer();
            });
        }

        function init() {
            _cacheElements();
            _createPlayer();
            _killPlayback();
            _stopPlayback();
        }

        function showScreenshot() {
            stopPlayer();
            elements.$screenshot
                .prev().css('z-index', '')
                .end()
                .animate({ opacity: 1 }, 300);
        }

        function stopPlayer() {
            player.pauseVideo();
        }

        function videoLoaded() {
            return isPlayerLoaded;
        }

        return {
            init: init,
            showScreenshot: showScreenshot,
            stopPlayer: stopPlayer,
            videoLoaded: videoLoaded
        }
    }

    if ($('.yt__player').length) {
        getYTScript().done(
            function () {
                $('.yt__player').each(function () {
                    var elem = $(this);
                    if (!elem.data('ytPlayer')) {
                        var p = ytPlayer(elem);
                        elem.data('ytPlayer', p);
                        p.init();
                    }
                });
            });
    }

})(window, $);
