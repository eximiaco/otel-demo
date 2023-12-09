# Plano de testes : Chamada de endpoint para xxxxxxxx

## Jornada

1. Realizar uma chamada para a API xxxxxx e aguardar retorno 200.
2. Verificar tamanho do retorno
3. Verificar tempo de resposta

### Objetivos

1. Medir tempo de reposta sob uma carga média
2. Tamanho do retorno deve ser aceitavel

## Cenários

### Warmup

Objetivo de fazer warmup da infra-estrutura antes do teste.

* Executor per-vu-iterations
* 5 VU Fixo
* 1 Iteração

### Load test

Objetivo de garantir a performance sob cargas distintas de usuários virtuais até o pico de concorrência esperado.

* Executor ramping-vus
* Step 1: 100 VUs e tempo total 2m
* Step 2: 200 VUs, Ramp-up 30s e tempo total 2m
* Step 3: 300 VUs, Ramp-up 30s e tempo total 2m
* Step 4: 400 VUs, Ramp-up 30s e tempo total 2m
* Step 5: 500 VUs, Ramp-up 30s e tempo total 2m
* Step 6: Ram-down 30s

#### Expectativas

1. Tempo médio de resposta de xxxxxxxxxx.
2. Banco não ultrapassar xxxxx % CPU
3. API não deve alocar mais que xxxx % de CPU
4. Vazão esperada deve ser entre  xxxx e xxx requisções/segundo

#### Ambiente de excução

1. Kubernetes Pod da API de 0.5 vcpu com 4gb
2. Banco Azure Sql com 500 DTU