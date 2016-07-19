var FollowingsService = function () {
    var addFollowing = function (artistId, done, fail) {
        $.post("/api/following", { artistId: artistId })
        .done(done)
        .fail(fail);
    };

    var deleteFollowing = function (artistId, done, fail) {
        $.ajax({
            url: "/api/following/" + artistId,
            method: "DELETE"
        })
        .done(done)
        .fail(fail);
    };

    return {
        addFollowing: addFollowing,
        deleteFollowing: deleteFollowing
    };
}();