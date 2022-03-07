var chaiVar = require("chai");
var chaiHttp = require("chai-http");
var server = require("https://localhost:44341/api/quotes");
//Assertion Style
chaiVar.should();
chaiVar.use(chaiHttp);
//describe('Tasks API', () => {
//    /**
//     * Test the GET route
//     */
//    describe("GET /api/tasks", () => {
//        it("It should GET all the tasks", (done) => {
//            chai.request(server)
//                .get("/api/tasks")
//                .end((err, response) => {
//                    response.should.have.status(200);
//                    response.body.should.be.a('array');
//                    response.body.length.should.be.eq(3);
//                    done();
//                });
//        });
//        it("It should NOT GET all the tasks", (done) => {
//            chai.request(server)
//                .get("/api/task")
//                .end((err, response) => {
//                    response.should.have.status(404);
//                    done();
//                });
//        });
//    });
//});
//# sourceMappingURL=MochaQuotesControllerTests.js.map