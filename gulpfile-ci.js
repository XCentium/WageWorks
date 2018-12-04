var gulp = require("gulp");
var gutil = require("gulp-util");
var debug = require("gulp-debug");
var foreach = require("gulp-foreach");
var rimrafDir = require("rimraf");
var rimraf = require("gulp-rimraf");
var util = require("gulp-util");
var runSequence = require("run-sequence");
var fs = require("fs");
var path = require("path");
var xmlpoke = require("xmlpoke");
var config = require("./gulpfile.js").config;
var unicorn = require("./scripts/unicorn.js");
var newer = require("gulp-newer");
var zip = require('gulp-zip');
var del = require('del');
var websiteRootBackup = config.websiteRoot;
var buildPath = path.resolve("./build");
var publishPath = path.resolve(buildPath + "/temp");
var tempPath = "D:\\temp";
var webpack_stream = require('webpack-stream')

gulp.task("CI-Publish", function (callback) {
    config.websiteRoot = publishPath;
    config.buildConfiguration = "Release";
    fs.mkdirSync(buildPath);
    fs.mkdirSync(publishPath);
    runSequence(
      "Publish-Foundation-Projects",
      "Publish-Feature-Projects",
      "Publish-Project-Projects",
      callback);
});

gulp.task("CI-Serialization-Zip", function (callback) {
    config.websiteRoot = tempPath;
    runSequence(
         "Copy-Serialized-Items",
        "Zip-Serialized-Items",
        "Delete-Serialized-Folders",
        "CI-Move-Serialization-File",
        callback);
});

gulp.task("CI-Move-Serialization-File", function (callback) {

    gulp.src(tempPath + '\\serialization.zip')
        .pipe(gulp.dest(publishPath + '/app_data/serialization'));

});


gulp.task("CI-Prepare-Package-Files", function (callback) {
    var excludeList = [
      config.websiteRoot + "\\bin\\{Sitecore,Unicorn,Lucene,Microsoft.Web.Infrastructure}*dll",
      config.websiteRoot + "\\compilerconfig.json.defaults",
      config.websiteRoot + "\\packages.config",
      //config.websiteRoot + "\\App_Config\\Include\\{Feature,Foundation,Project}\\*Serialization.config",
      config.websiteRoot + "\\App_Config\\Include\\{Feature,Foundation,Project}\\zzz\\zzz.*Dev.Settings.config",
      config.websiteRoot + "\\App_Config\\Include\\Unicorn\\*",
      "!" + config.websiteRoot + "\\bin\\Sitecore.Support*dll"
    ];
    console.log(excludeList);

    return gulp.src(excludeList, { read: false }).pipe(rimraf({ force: true }));
});

gulp.task("CI-Enumerate-Files", function () {
    var packageFiles = [];
    config.websiteRoot = websiteRootBackup;

    return gulp.src(publishPath + "/**/*.*", { base: "temp", read: false })
      .pipe(foreach(function (stream, file) {
          var item = "/" + file.relative.replace(/\\/g, "/");
          console.log("Added to the package:" + item);
          packageFiles.push(item);
          return stream;
      })).pipe(gutil.buffer(function () {
          xmlpoke("./package.xml", function (xml) {
              for (var idx in packageFiles) {
                  xml.add("project/Sources/xfiles/Entries/x-item", packageFiles[idx]);
              }
          });
      }));
});

gulp.task("CI-Enumerate-Items", function () {
    var itemPaths = [];
    var allowedPatterns = [
      "./src/**/serialization/**/*.yml",
      "!./src/**/serialization/*.Roles/**/*.yml",
      "!./src/**/serialization/*.Users/**/*.yml"
    ];
    return gulp.src(allowedPatterns)
        .pipe(foreach(function (stream, file) {
            console.log(file);
            var itemPath = unicorn.getFullItemPath(file);
            itemPaths.push(itemPath);
            return stream;
        })).pipe(gutil.buffer(function () {
            xmlpoke("./package.xml", function (xml) {
                for (var idx in itemPaths) {
                    xml.add("project/Sources/xitems/Entries/x-item", itemPaths[idx]);
                }
            });
        }));
});

gulp.task("CI-Enumerate-Users", function () {
    var users = [];

    return gulp.src("./src/**/serialization/*.Users/**/*.yml")
        .pipe(foreach(function (stream, file) {
            console.log(file);
            var fileContent = file.contents.toString();
            var userName = unicorn.getUserPath(file);
            users.push(userName);
            return stream;
        })).pipe(gutil.buffer(function () {
            xmlpoke("./package.xml", function (xml) {
                for (var idx in users) {
                    xml.add("project/Sources/accounts/Entries/x-item", users[idx]);
                }
            });
        }));
});

gulp.task("CI-Enumerate-Roles", function () {
    var roles = [];

    return gulp.src("./src/**/serialization/*.Roles/**/*.yml")
        .pipe(foreach(function (stream, file) {
            console.log(file);
            var fileContent = file.contents.toString();
            var roleName = unicorn.getRolePath(file);
            roles.push(roleName);
            return stream;
        })).pipe(gutil.buffer(function () {
            xmlpoke("./package.xml", function (xml) {
                for (var idx in roles) {
                    xml.add("project/Sources/accounts/Entries/x-item", roles[idx]);
                }
            });
        }));
});

gulp.task("CI-Clean", function (callback) {
    rimrafDir.sync(buildPath);
    callback();
});

gulp.task("CI-Do-magic", function (callback) {
    runSequence(
        "CI-Clean",        
        "CI-Publish",
        "CI-Prepare-Package-Files",
        "CI-Enumerate-Files",
        //"CI-Enumerate-Items",
        //"CI-Enumerate-Users",
        //"CI-Enumerate-Roles",
        callback);
});

gulp.task("CI-Copy-Serialized-Items", function () {
    var root = "./src";
    var roots = [root + "/**/Serialization", "!" + root + "/**/obj/**/Serialization"];
    var files = "/**/*.yml";
    var destination = publishPath + "/App_Data/Serialization";
    return gulp.src(roots, { base: root }).pipe(
      foreach(function (stream, file) {
          console.log("Publishing from " + file.path);
          var index = file.path.indexOf("\\src") + 4;
          var itemPath = file.path.substring(index);
          var finaldestination = destination + itemPath;
          gulp.src(file.path + files, { base: file.path })
            .pipe(newer(finaldestination))
            .pipe(debug({ title: "Copying " }))
            .pipe(gulp.dest(finaldestination));
          return stream;
      })

     );
});

gulp.task("CI-Zip-Serialized-Items", function () {
    var destination = publishPath + "\\App_Data\\Serialization";

    gulp.src(destination + "/**")
        .pipe(zip('serialization.zip'))
        .pipe(gulp.dest(destination));

});

gulp.task("CI-Delete-Serialized-Folders", function () {
    var target = publishPath + "\\App_Data\\Serialization";
    var feature = target + "\\feature";
    var foundation = target + "\\foundation";
    var project = target + "\\project";

    del([feature], { force: true })
        .then(paths => {
            console.log('Deleted files and folders:\n', paths.join('\n'));
        });
    del([foundation], { force: true })
        .then(paths => {
            console.log('Deleted files and folders:\n', paths.join('\n'));
        });
    del([project], { force: true })
        .then(paths => {
            console.log('Deleted files and folders:\n', paths.join('\n'));
        });

});

