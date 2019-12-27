module.exports = async function (context, req) {
    var rand = Math.random();
    if (rand >= 0.9) {
        throw "A worker has cursed on the voyage"
    }
    context.res = {
        status: 200
    };
};