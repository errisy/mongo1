"use strict";
var mongodb = require("mongodb");
var assert = require("assert");
console.log('test');
var url = 'mongodb://localhost:27017/myproject';
var MongoClient = mongodb.MongoClient;
var insertDocuments = function (db, callback) {
    //get the documents collection
    var collection = db.collection('documents');
    //insert documents
    collection.insertMany([
        { a: 1 },
        { a: 2 },
        { a: 3 }
    ], function (err, result) {
        assert.equal(err, null);
        assert.equal(3, result.result.n);
        assert.equal(3, result.ops.length);
        console.log("Inserted 3 documents into the collection");
        callback(result);
    });
};
MongoClient.connect(url, function (err, db) {
    assert.equal(null, err);
    console.log("connected successfully");
    insertDocuments(db, function (result) {
        db.close();
    });
});
////Lets require/import the HTTP module
//import * as http from "http";
////Lets define a port we want to listen to
//const PORT = 8080;
////We need a function which handles requests and send response
//function handleRequest(request, response) {
//    response.end('It Works!! Path Hit: ' + request.url);
//}
////Create a server
//var server = http.createServer(handleRequest);
////Lets start our server
//server.listen(PORT, function () {
//    //Callback triggered when server is successfully listening. Hurray!
//    console.log("Server listening on: http://localhost:%s", PORT);
//}); 
//# sourceMappingURL=node.js.map