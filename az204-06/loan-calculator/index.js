module.exports = async function (context, req) {
    context.log('JavaScript HTTP trigger function processed a request.');

    // Try to grab principal, rate and term from the query string and
    // parse them as numbers
    const principal = parseFloat(req.query.principal);
    const rate = parseFloat(req.query.rate);
    const term = parseFloat(req.query.term);

    if ([principal, rate, term].some(isNaN)) {
        // If any empty or non-numeric values, return a 400 response with an
        // error message
        context.res = {
            status: 400,
            body: "Please supply principal, rate and term in the query string"
        };
    } else {
        // Otherwise set the response body to the product of the three values
        context.res = { body: Number((principal * rate * term).toFixed()) };
    }
}