var GigDetailsController = function (followingsService) {
    var button;

    var init = function (container) {
        $(container).on("click", ".js-toggle-following", toggleFollowing);
    };

    var toggleFollowing = function (e) {
        button = $(e.target);

        var artistId = button.data("artist-id");

        if ($(button).hasClass("btn-default"))
            followingsService.addFollowing(artistId, done, fail);
        else
            followingsService.deleteFollowing(artistId, done, fail);
    };

    var done = function () {
        var text = (button.text() == "Following") ? "Follow" : "Following";

        button.toggleClass("btn-info").toggleClass("btn-default").text(text);
    };

    var fail = function() {
        alert("Something failed");
    };

    return {
        init: init
    }

}(FollowingsService);