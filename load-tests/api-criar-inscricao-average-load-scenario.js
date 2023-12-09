import http from 'k6/http';
import { uuidv4, randomIntBetween  } from 'https://jslib.k6.io/k6-utils/1.4.0/index.js';
import { check, sleep } from 'k6';
import { SharedArray } from 'k6/data';
import papaparse from 'https://jslib.k6.io/papaparse/5.1.1/index.js';
import { Rate, Trend, Counter } from 'k6/metrics';
import exec from 'k6/execution';

const BASE_URL = 'https://localhost:7149/api/v1';

const csvData = new SharedArray('data', function () {
    // Load CSV file and parse it using Papa Parse
    return papaparse.parse(open('./data.csv'), { header: true }).data;
  });

const scenarios = {
    warmup: {
        executor: "per-vu-iterations",
        vus: 5,
        iterations: 1,
        startTime: "0s"
    },
    load_test: {
        executor: 'ramping-vus',
        startVUs: 50,
        stages: [
            { duration: '2m', target: 100 },
            { duration: '30s', target: 200 },
            { duration: '2m', target: 200 },
            { duration: '30s', target: 300 },
            { duration: '2m', target: 300 },
            { duration: '30s', target: 400 },
            { duration: '2m', target: 400 },
            { duration: '30s', target: 500 },
            { duration: '2m', target: 500 },
            { duration: '30s', target: 0 },
        ],
    },
    breaking_users: {
        executor: 'ramping-vus',
        startVUs: 5,
        stages: [
            { duration: '30s', target: 5 },
            { duration: '5s', target: 10 },
            { duration: '30s', target: 10 },
            { duration: '5s', target: 15 },
            { duration: '30s', target: 15 },
            { duration: '5s', target: 20 },
            { duration: '30s', target: 20 },
            { duration: '5s', target: 25 },
            { duration: '30s', target: 25 },
            { duration: '5s', target: 30 },
            { duration: '30s', target: 30 },
            { duration: '5s', target: 35 },
            { duration: '30s', target: 35 },
            { duration: '5s', target: 40 },
            { duration: '30s', target: 40 },
            { duration: '5s', target: 45 },
            { duration: '30s', target: 45 },
            { duration: '5s', target: 50 },
            { duration: '30s', target: 50 },
            { duration: '30s', target: 0 }
        ],
    },
    constant_iterations:{
        executor: 'constant-arrival-rate',
        duration: '3m',
        rate: 15,
        preAllocatedVUs: 40,
        maxVUs: 1000
    }
    /* ,all_data: {
        executor: 'shared-iterations',
        vus: 10,
        iterations: data.length,
        maxDuration: '1h',
    }
    ,vus_same_all_data: {
        executor: 'per-vu-iterations',
        vus: data.length,
        iterations: 1,
        maxDuration: '1h30m',
      } */
};

export const options = {
    scenarios: {}
  };

if(__ENV.scenario)
    options.scenarios[__ENV.scenario] = scenarios[__ENV.scenario];
else
    options.scenarios = scenarios;

export default () => {

    //const randomUser = csvData[Math.floor(Math.random() * csvData.length)];

    const params = {
        headers: {
            //"x-correlation-id": eventUUID,
            "Content-Type": "application/json"
        }
    };

    const payload = {
        CpfAluno: "123456789",
        CpfResponsavel: "123456789",
        CodigoTurma: 2
    };
    
    const createInscricaoResponse = http.post(`${BASE_URL}/Inscricoes`, JSON.stringify(payload), params);    
    
    if(__ENV.scenario === "warmup")
        console.log(createInscricaoResponse.status);
        
    check(createInscricaoResponse, {
        'is status 200': (r) => r.status === 200,
        //'body size is 11,105 bytes': (r) => r.body.length == 11105,
        'response time is above 300 ms' : (r) => r.timings.duration <= 300
      });
};
