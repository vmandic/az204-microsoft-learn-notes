module.exports = async function (context, req, connectionInfo) {
    context.res.body = connectionInfo;
    // NOTE: As the function is called, the SignalR connection is returned as the response to the function.
}